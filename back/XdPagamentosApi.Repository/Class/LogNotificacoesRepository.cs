using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Domain.Models.NotificacaoTransacao;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class LogNotificacoesRepository : Base<LogNotificacoes>, ILogNotificacoesRepository
    {
        private readonly MySqlContext _mySqlContext;

        public LogNotificacoesRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public async Task<bool> GerarOrdemPagamento(DtoTransactionPagSeguro dtoTransactionPagSeguro, string estabelecimento)
        {

            var transacao = new Transacao();

            PreencheValoresPadroes(dtoTransactionPagSeguro, estabelecimento, transacao);

            //Busca o tipo da operadora
            await BuscaTipoOperadora(transacao);

            //Busca o status da transacao
            await BuscaDescricaoStatusTransacao(transacao, dtoTransactionPagSeguro);

            //Busca o tipo de operacao
            await BuscaTipoOperacao(transacao, dtoTransactionPagSeguro);

            if (string.IsNullOrEmpty(transacao.NumTerminal))
            {
                return await SalvarTransacao(transacao);
            }
            else
            {
                //Consulta se o numero de terminal e validado
                var respostaTerminal = await _mySqlContext.Terminais.Where(c => c.NumTerminal.Trim().Equals(transacao.NumTerminal.Trim())).AsNoTracking().Include(c => c.ListaRelClienteTerminal).FirstOrDefaultAsync();

                //Se nao achar o terminal na base salva transacao
                if (respostaTerminal == null)
                    return await SalvarTransacao(transacao);

                //Se nao tiver o terminal relacionado com cliente salva a transacao como esta
                if (respostaTerminal.ListaRelClienteTerminal.Count() == 0)
                    return await SalvarTransacao(transacao);

                transacao.CliId = respostaTerminal.ListaRelClienteTerminal.FirstOrDefault().CliId;
                transacao.DtGravacao = DateTime.Now;

                //Busca o tipo de transacao de acordo de quantidade de parcelas
                var respostaTipoTransacao = await _mySqlContext.TipoTransacoes.Where(c => c.CliId.Equals(transacao.CliId) && c.QtdParcelas.Equals(transacao.QtdParcelas) && c.Status.Equals("A")).AsNoTracking().FirstOrDefaultAsync();

                if (respostaTipoTransacao != null)
                    transacao.TitPercDesconto = respostaTipoTransacao.PercDesconto;

                //Gera Ordem de Pagto
                return  await GeraOrdemPagto(transacao) && await GeraGestaoPagamento(transacao, dtoTransactionPagSeguro) && await AtualizaTransacao(transacao);

            }

        }

        private static void PreencheValoresPadroes(DtoTransactionPagSeguro dtoTransactionPagSeguro, string estabelecimento, Transacao transacao)
        {
            transacao.DtOperacao = dtoTransactionPagSeguro.Date;
            transacao.DtCredito = dtoTransactionPagSeguro.EscrowEndDate;
            transacao.VlBruto = dtoTransactionPagSeguro.GrossAmount.ToString();
            transacao.VlLiquido = dtoTransactionPagSeguro.NetAmount.ToString();
            transacao.EstId = Convert.ToInt32(estabelecimento);
            transacao.NumTerminal = dtoTransactionPagSeguro.DeviceInfo?.SerialNumber;
            transacao.NumCartao = $"** ** ** {dtoTransactionPagSeguro.DeviceInfo?.Holder}";
            transacao.QtdParcelas = dtoTransactionPagSeguro.PaymentMethod.Type.Equals("8") ? "00" : dtoTransactionPagSeguro.InstallmentCount.ToString().PadLeft(2, '0');
            transacao.Chave = $"{dtoTransactionPagSeguro.PrimaryReceiver.PublicKey}/{dtoTransactionPagSeguro.Code}";
            transacao.CodAutorizacao = dtoTransactionPagSeguro.DeviceInfo.Reference;
            transacao.TipoTransacao = "01";
            transacao.OrigemAjuste = "";
            transacao.MeioCaptura = dtoTransactionPagSeguro.Items.Item.Description;
            transacao.TaxaComissaoOperador = ((Convert.ToDecimal(transacao.VlBruto) - Convert.ToDecimal(transacao.VlLiquido)) * 100 / Convert.ToDecimal(transacao.VlBruto)).ToString();
            transacao.PagId = 0;
            transacao.HisId = 0;
            transacao.CliId = 0;
            transacao.TitPercDesconto = "";
            transacao.EstId = Convert.ToInt32(estabelecimento);
            transacao.Descricao = "";
        }

        private async Task BuscaTipoOperadora(Transacao transacao)
        {
            var resposta = await _mySqlContext.Estabelecimentos.Where(c => c.Id.Equals(transacao.EstId)).AsNoTracking().FirstOrDefaultAsync();

            if (resposta != null)
                transacao.OpeId = resposta.OpeId;
        }

        private async Task BuscaTipoOperacao(Transacao transacao, DtoTransactionPagSeguro dtoTransactionPagSeguro)
        {
            var resposta = await _mySqlContext.TipoOperacoes.Where(c => c.Codigo.Equals(dtoTransactionPagSeguro.PaymentMethod.Code.ToString())).AsNoTracking().FirstOrDefaultAsync();

            if (resposta != null)
                transacao.Descricao = resposta.Descricao;
        }

        private async Task BuscaDescricaoStatusTransacao(Transacao transacao, DtoTransactionPagSeguro dtoTransactionPagSeguro)
        {
            var resposta = await _mySqlContext.StatusTransacoes.Where(c => c.Codigo.Equals(dtoTransactionPagSeguro.Status.ToString()) ).AsNoTracking().FirstOrDefaultAsync();

            if (resposta != null)
                transacao.Status = resposta.DescStatus;
            else
                transacao.Status = "vazio";
        }

        private async Task<string> BuscaStatusTransacao(Transacao transacao, DtoTransactionPagSeguro dtoTransactionPagSeguro)
        {
            var resposta = await _mySqlContext.StatusTransacoes.Where(c => c.Codigo.Equals(dtoTransactionPagSeguro.Status.ToString())).AsNoTracking().FirstOrDefaultAsync();

            if (resposta != null)
                return resposta.Tipo;

            return "";
        }

        private async Task<int> BuscaContaEstabelecimento(int estId)
        {
            var resposta = await _mySqlContext.RelContaEstabelecimentos.Where(c => c.EstId.Equals(estId) && c.CreditoAutomatico.Equals("S")).AsNoTracking().FirstOrDefaultAsync();

            if (resposta != null)
                return resposta.Id;

            return 0;
        }

        private async Task<bool> GeraOrdemPagto(Transacao transacao)
        {
            try
            {
                var pagamento = new Pagamentos
                {
                    CliId = transacao.CliId,
                    Data = transacao.DtOperacao,
                    ListaTransacoes = new List<Transacao>() { transacao }
                };

                var ordemPagto = new OrdemPagto
                {
                    EstId = transacao.EstId,
                    DtEmissao = transacao.DtOperacao,
                    DtCredito = transacao.DtCredito,
                    Valor = Convert.ToDecimal(transacao.VlLiquido).ToString(),
                    ListaPagamentos = new List<Pagamentos> { pagamento },
                    Status = "NP"
                };

                _mySqlContext.OrdemPagtos.Add(ordemPagto);
                return await _mySqlContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private async Task<bool> GeraGestaoPagamento(Transacao transacao, DtoTransactionPagSeguro dtoTransactionPagSeguro) {
            try
            {
                var gestaoPagamento = new GestaoPagamento
                {
                    DtHrLancamento = transacao.DtOperacao,
                    DtHrCredito = transacao.DtCredito,
                    Descricao = "Pagseguro",
                    Tipo = await BuscaStatusTransacao(transacao, dtoTransactionPagSeguro),
                    VlBruto = transacao.VlBruto,
                    VlLiquido = transacao.VlLiquido,
                    Grupo = "EG",
                    FopId = 2,
                    CliId = transacao.CliId,
                    RceId = await BuscaContaEstabelecimento(transacao.EstId)
                };

                _mySqlContext.Add(gestaoPagamento);
                return await _mySqlContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SalvarTransacao(Transacao transacao)
        {
            try
            {
                _mySqlContext.Add(transacao);
                return await _mySqlContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> AtualizaTransacao(Transacao transacao)
        {
            try
            {
                _mySqlContext.Update(transacao);
                return await _mySqlContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}
