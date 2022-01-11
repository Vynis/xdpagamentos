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
                    //Cadastrar ordem de Pagamento e Pagamento
                    var pagamento = new Pagamentos
                    {
                        CliId = parametros.IdCliente,
                        Data = parametros.DataLancamentoCredito,
                    };

                    var listaPagamento = new List<Pagamentos>();
                    listaPagamento.Add(pagamento);

                    decimal valorLiquidoTotal = 0;
                    decimal valorBrutoTotal = 0;

                    parametros.TerminaisSelecionados.Where(x => x.NumTerminal == item.NumTerminal).ToList().ForEach(x => x.ListaTransacoes.ForEach(c => valorLiquidoTotal += Convert.ToDecimal(c.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," })));
                    parametros.TerminaisSelecionados.Where(x => x.NumTerminal == item.NumTerminal).ToList().ForEach(x => x.ListaTransacoes.ForEach(c => valorBrutoTotal += Convert.ToDecimal(c.VlBruto, new NumberFormatInfo() { NumberDecimalSeparator = "," })));

                    decimal valorTotal = valorBrutoTotal - valorLiquidoTotal;

                    var idEstabelecimento = item.ListaTransacoes.FirstOrDefault().EstId;


                    var ordemPagto = new OrdemPagto
                    {
                        FopId = 0,
                        Valor = valorTotal.ToString(),
                        Status = "NP",
                        DtEmissao = parametros.DataLancamentoCredito,
                        EstId = idEstabelecimento,
                        //ListaPagamentos = listaPagamento
                    };

                    _mySqlContext.OrdemPagtos.Add(ordemPagto);
                    await _mySqlContext.SaveChangesAsync();

                    //Atualiza a tabela ta de transacoes com id do pagamento
                    //var idPagamento = ordemPagto.ListaPagamentos.FirstOrDefault().Id;

                    //foreach (var transacao in item.ListaTransacoes)
                    //{
                    //    var transacaoBD = await _mySqlContext.Transacoes.Where(c => c.Id.Equals(transacao.Id)).AsNoTracking().FirstOrDefaultAsync();

                    //    transacaoBD.PagId = idPagamento;

                    //    _mySqlContext.Transacoes.Update(transacaoBD);
                    //    await _mySqlContext.SaveChangesAsync();
                    //}
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
    }
}
