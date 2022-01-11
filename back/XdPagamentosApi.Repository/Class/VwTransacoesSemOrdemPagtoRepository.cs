using FiltrDinamico.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class VwTransacoesSemOrdemPagtoRepository : Base<VwTransacoesSemOrdemPagto>, IVwTransacoesSemOrdemPagtoRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public VwTransacoesSemOrdemPagtoRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<bool> Gerar(ParamOrdemPagto parametros)
        {

            try
            {
                if (parametros.TerminaisSelecionados.Count() == 0)
                    return false;


                foreach (var item in parametros.TerminaisSelecionados)
                {
                    var listaTransacoes = new List<Transacao>();
                    item.ListaTransacoes.ForEach(x => listaTransacoes.Add(BuscaTransacao(x.Id).Result));

                    foreach (var transacao in listaTransacoes)
                    {
                        transacao.CliId = parametros.IdCliente;

                        var tipoValor = await BuscaStatusTransacao(transacao.StatusCodigo);

                        var vlLiquido = tipoValor.Equals("D") ? $"-{transacao.VlLiquido}" : transacao.VlLiquido.ToString();


                        //Tabela de Pagamentos
                        var pagamento = new Pagamentos
                        {
                            CliId = parametros.IdCliente,
                            Data = transacao.DtOperacao,
                        };

                        //Tabela de Ordem de Pagamento
                        var  ordemPagto = new OrdemPagto
                        {
                            EstId = transacao.EstId,
                            DtEmissao = transacao.DtOperacao,
                            DtCredito = transacao.DtCredito,
                            Valor = vlLiquido,
                            ListaPagamentos = new List<Pagamentos> { pagamento },
                            Status = "NP"
                        };

                        //Salva a tabela de Ordem de Pagamento e Pagamento
                        _mySqlContext.OrdemPagtos.Add(ordemPagto);
                        await _mySqlContext.SaveChangesAsync();

                        //Tabela de Gestao de Pagamento
                        var gestaoPagamento = new GestaoPagamento
                        {
                            DtHrLancamento = transacao.DtOperacao,
                            DtHrCredito = parametros.DataLancamentoCredito,
                            Descricao = $"Ordem de pagamento - {ordemPagto.Id}",
                            Tipo = await BuscaStatusTransacao(transacao.StatusCodigo),
                            VlBruto = "0,00",
                            VlLiquido = transacao.VlLiquido.Replace("-", ""),
                            ValorSolicitadoCliente = "0,00",
                            Grupo = "EC",
                            FopId = 2,
                            CliId = transacao.CliId,
                            RceId = parametros.IdConta,
                            CodRef = $"ORPID{ordemPagto.Id}",
                            Status = "AP"
                        };

                        //Salva na tabela de Gestao de Pagamento
                        _mySqlContext.Add(gestaoPagamento);
                        await _mySqlContext.SaveChangesAsync();

                        transacao.PagId = pagamento.Id;
                        transacao.DtGravacao = DateTime.Now;

                        //Atualiza a tabela de transacoes
                        _mySqlContext.Update(transacao);
                        await _mySqlContext.SaveChangesAsync();
                    }


                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }

        public async Task<List<TransacoesSemOrdemPagtoPorCliente>> ListaTransacoesSemOrdemPagto(PaginationFilter paginationFilter)
        {

            Expression<Func<VwTransacoesSemOrdemPagto, bool>> expressionDynamic = p => p.Id != 0;

            var transacoes = new List<VwTransacoesSemOrdemPagto>();

            if (paginationFilter.Filtro.Count() > 0)
            {
                expressionDynamic = _filtroDinamico.FromFiltroItemList<VwTransacoesSemOrdemPagto>(paginationFilter.Filtro.ToList());

                IQueryable<VwTransacoesSemOrdemPagto> query = _mySqlContext.VwTransacoesSemOrdemPagtos.Where(expressionDynamic);

                transacoes = await query.AsNoTracking().ToListAsync();
            }
            else
                transacoes = await _mySqlContext.VwTransacoesSemOrdemPagtos.ToListAsync();

            var clientes = await _mySqlContext.Clientes.Where(x => x.Status.Equals("A")).Include(c => c.Estabelecimento).AsNoTracking().ToListAsync();

            var listaTransacoesSemOrdem = new List<TransacoesSemOrdemPagtoPorCliente>();

            foreach (var cliente in clientes)
            {
                var itemTransacoesSemOrdem = new TransacoesSemOrdemPagtoPorCliente
                {
                    IdCliente = cliente.Id,
                    NomeCliente = cliente.Nome,
                    NomeEstabelcimento = cliente.Estabelecimento.Nome,
                    NumEstabelecimento = cliente.Estabelecimento.NumEstabelecimento,
                    ListaTransacoes = transacoes.Where(x => x.ClidId.Equals(cliente.Id)).ToList()
                };

                if (transacoes.Where(x => x.ClidId.Equals(cliente.Id)).ToList().Count() > 0)
                    listaTransacoesSemOrdem.Add(itemTransacoesSemOrdem);
            }

            var transacoesSemCliente = transacoes.Where(x => x.ClidId.Equals(0)).ToList();
            var estabelecimentosSemCliente = transacoes.Where(x => x.ClidId.Equals(0)).GroupBy(x => x.EstId).Select(x => new { EstId = x.Key });

            if (transacoesSemCliente.Count() > 0)
            {
                foreach (var estabelecimento in estabelecimentosSemCliente)
                {

                    var buscaEstabelecimento = _mySqlContext.Estabelecimentos.Where(x => x.Id.Equals(estabelecimento.EstId)).ToListAsync().Result.FirstOrDefault();

                    var itemTransacoesSemOrdem = new TransacoesSemOrdemPagtoPorCliente
                    {
                        IdCliente = 0,
                        NomeCliente = "Cliente não identificado",
                        NomeEstabelcimento = buscaEstabelecimento.Nome,
                        NumEstabelecimento = buscaEstabelecimento.NumEstabelecimento,
                        ListaTransacoes = transacoes.Where(x => x.ClidId.Equals(0) && x.EstId.Equals(estabelecimento.EstId)).ToList()
                    };

                    listaTransacoesSemOrdem.Add(itemTransacoesSemOrdem);
                }


            }

            return listaTransacoesSemOrdem;
        }

        public async Task<List<TransacoesSemOrdemPagtoPorTerminal>> ListaTransacoesSemOrdemPagtoTerminal(PaginationFilter paginationFilter)
        {

            Expression<Func<VwTransacoesSemOrdemPagto, bool>> expressionDynamic = p => p.Id != 0;

            var transacoes = new List<VwTransacoesSemOrdemPagto>();

            if (paginationFilter.Filtro.Count() > 0)
            {
                expressionDynamic = _filtroDinamico.FromFiltroItemList<VwTransacoesSemOrdemPagto>(paginationFilter.Filtro.ToList());

                IQueryable<VwTransacoesSemOrdemPagto> query = _mySqlContext.VwTransacoesSemOrdemPagtos.Where(expressionDynamic);

                transacoes = await query.AsNoTracking().ToListAsync();  
            }
            else
                transacoes = await _mySqlContext.VwTransacoesSemOrdemPagtos.ToListAsync();

            var grupoTerminal = transacoes.GroupBy(c => c.NumTerminal).Select(c => new { NumTerminal = c.Key });

            var listaTransacaoPorTerminal = new List<TransacoesSemOrdemPagtoPorTerminal>();

            grupoTerminal.ToList().ForEach(c => listaTransacaoPorTerminal.Add(new TransacoesSemOrdemPagtoPorTerminal { NumTerminal = c.NumTerminal, ListaTransacoes = transacoes.ToList().Where(x => x.NumTerminal.Equals(c.NumTerminal)).ToList() }));

            return listaTransacaoPorTerminal;
        }

        public async Task<Transacao> BuscaTransacao(int Id)
        {
            try
            {
                var transacao = await _mySqlContext.Transacoes.Where(x => x.Id.Equals(Id)).FirstOrDefaultAsync();

                return transacao;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<string> BuscaStatusTransacao(int Id)
        {
            var resposta = await _mySqlContext.StatusTransacoes.Where(c => c.Id.Equals(Id)).AsNoTracking().FirstOrDefaultAsync();

            if (resposta != null)
                return resposta.Tipo;

            return "";
        }



    }
}
