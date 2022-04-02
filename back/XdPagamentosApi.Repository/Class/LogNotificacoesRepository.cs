using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Domain.Models.NotificacaoTransacao;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;
using XdPagamentoApi.Shared.Helpers;

namespace XdPagamentosApi.Repository.Class
{
    public class LogNotificacoesRepository : Base<LogNotificacoes>, ILogNotificacoesRepository
    {
        private readonly MySqlContext _mySqlContext;
        private OrdemPagto ordemPagto;

        public LogNotificacoesRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public async Task<bool> GerarOrdemPagamento(DtoTransactionPagSeguro dtoTransactionPagSeguro, string estabelecimento)
        {
            if (! await ValidaTransacao(dtoTransactionPagSeguro))
                return false;

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

                var respostTipoTransacaoPadrao = await _mySqlContext.TipoTransacoes.Where(c => c.CliId.Equals(0) && c.QtdParcelas.Equals(transacao.QtdParcelas) && c.Status.Equals("A")).AsNoTracking().FirstOrDefaultAsync();

                if (respostTipoTransacaoPadrao != null)
                    transacao.TitPercDesconto = respostTipoTransacaoPadrao.PercDesconto;

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
                return  await GeraOrdemPagto(transacao, dtoTransactionPagSeguro) && await GeraGestaoPagamento(transacao, dtoTransactionPagSeguro) && await AtualizaTransacao(transacao);

            }

        }

        private static void PreencheValoresPadroes(DtoTransactionPagSeguro dtoTransactionPagSeguro, string estabelecimento, Transacao transacao)
        {
            transacao.DtOperacao = dtoTransactionPagSeguro.Date;
            transacao.DtCredito = dtoTransactionPagSeguro.EscrowEndDate;
            transacao.VlBruto = HelperFuncoes.ValorMoedaBRDouble(dtoTransactionPagSeguro.GrossAmount);
            transacao.VlLiquido = HelperFuncoes.ValorMoedaBRDouble(dtoTransactionPagSeguro.NetAmount);
            transacao.EstId = Convert.ToInt32(estabelecimento);
            transacao.NumTerminal = dtoTransactionPagSeguro.DeviceInfo?.SerialNumber;
            transacao.NumCartao = $"** ** ** {dtoTransactionPagSeguro.DeviceInfo?.Holder}";
            transacao.QtdParcelas = dtoTransactionPagSeguro.PaymentMethod.Type.Equals("8") ? "00" : dtoTransactionPagSeguro.InstallmentCount.ToString().PadLeft(2, '0');
            transacao.Chave = $"{dtoTransactionPagSeguro.PrimaryReceiver.PublicKey}/{dtoTransactionPagSeguro.Code}";
            transacao.CodAutorizacao = dtoTransactionPagSeguro.DeviceInfo.Reference;
            transacao.TipoTransacao = "01";
            transacao.OrigemAjuste = "";
            transacao.MeioCaptura = dtoTransactionPagSeguro.Items.Item.Description;
            transacao.TaxaComissaoOperador = HelperFuncoes.ValorMoedaBRDouble((Convert.ToDouble(transacao.VlBruto) - Convert.ToDouble(transacao.VlLiquido)) * 100 / Convert.ToDouble(transacao.VlBruto)) ;
            transacao.PagId = 0;
            transacao.HisId = 0;
            transacao.CliId = 0;
            transacao.TitPercDesconto = "";
            transacao.EstId = Convert.ToInt32(estabelecimento);
            transacao.Descricao = "";
            transacao.StatusCodigo = dtoTransactionPagSeguro.Status;
            transacao.DtGravacao = null;
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

        private async Task<string> BuscaStatusTransacao(DtoTransactionPagSeguro dtoTransactionPagSeguro)
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

        private async Task<bool> ValidaTransacao(DtoTransactionPagSeguro dtoTransactionPagSeguro)
        {

            if (!dtoTransactionPagSeguro.Status.Equals(3) && !dtoTransactionPagSeguro.Status.Equals(4) && !dtoTransactionPagSeguro.Status.Equals(6))
                return false;

            var chave = $"{dtoTransactionPagSeguro.PrimaryReceiver.PublicKey}/{dtoTransactionPagSeguro.Code}";

            if (dtoTransactionPagSeguro.Status.Equals(6))
            {
                var validaCancelado = await _mySqlContext.Transacoes.Where(c => c.Chave.Equals(chave) && c.StatusCodigo.Equals(6) ).AsNoTracking().FirstOrDefaultAsync();

                if (validaCancelado != null)
                    return false;

                return true;
            }

            var valida = await _mySqlContext.Transacoes.Where(c => c.Chave.Equals(chave) && ( c.StatusCodigo.Equals(3) || c.StatusCodigo.Equals(4)) ).AsNoTracking().FirstOrDefaultAsync();

            if (valida != null)
                return false;

            return true;
        }

        private async Task<bool> GeraOrdemPagto(Transacao transacao, DtoTransactionPagSeguro dtoTransactionPagSeguro)
        {
            try
            {

                var tipoValor = await BuscaStatusTransacao(dtoTransactionPagSeguro);

                var vlLiquido = tipoValor.Equals("D") ?  $"-{transacao.VlLiquido}" : transacao.VlLiquido.ToString();

                var pagamento = new Pagamentos
                {
                    CliId = transacao.CliId,
                    Data = transacao.DtOperacao,
                    ListaTransacoes = new List<Transacao>() { transacao }
                };

                pagamento.ListaTransacoes.ForEach(x => x.VlLiquido = vlLiquido);

                ordemPagto = new OrdemPagto
                {
                    EstId = transacao.EstId,
                    DtEmissao = transacao.DtOperacao,
                    DtCredito = transacao.DtCredito,
                    Valor =  vlLiquido ,
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
                    Descricao = $"Ordem de pagamento - {ordemPagto.Id}",
                    Tipo = await BuscaStatusTransacao(dtoTransactionPagSeguro),
                    VlBruto = "0,00",
                    VlLiquido = transacao.VlLiquido.Replace("-", ""),
                    ValorSolicitadoCliente = "0,00",
                    Grupo = "EC",
                    FopId = 2,
                    CliId = transacao.CliId,
                    RceId = await BuscaContaEstabelecimento(transacao.EstId),
                    CodRef = $"ORPID{ordemPagto.Id}",
                    Status = "AP"
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
