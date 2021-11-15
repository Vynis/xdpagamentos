using FiltrDinamico.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                if (parametros.ClientesSelecionados.Count() == 0)
                    return false;


                foreach (var item in parametros.ClientesSelecionados)
                {
                    //Cadastrar ordem de Pagamento e Pagamento
                    var pagamento = new Pagamentos
                    {
                        CliId = item.IdCliente,
                        Data = parametros.DataLancamentoCredito,
                    };

                    var listaPagamento = new List<Pagamentos>();
                    listaPagamento.Add(pagamento);

                    decimal valorLiquidoTotal = 0;

                    parametros.ClientesSelecionados.Where(x => x.IdCliente == item.IdCliente).ToList().ForEach(x => x.ListaTransacoes.ForEach(c => valorLiquidoTotal += Convert.ToDecimal(c.VlLiquido)));

                    var idEstabelecimento = (await _mySqlContext.Estabelecimentos.Where(c => c.NumEstabelecimento == item.NumEstabelecimento).AsNoTracking().FirstOrDefaultAsync()).Id;

                    var ordemPagto = new OrdemPagto
                    {
                        FopId = 0,
                        Valor = valorLiquidoTotal.ToString(),
                        Status = "NP",
                        DtEmissao = DateTime.Now,
                        EstId = idEstabelecimento,
                        ListaPagamentos = listaPagamento
                    };

                    _mySqlContext.OrdemPagtos.Add(ordemPagto);
                    await _mySqlContext.SaveChangesAsync();

                    //Atualiza a tabela ta de transacoes com id do pagamento
                    var idPagamento = ordemPagto.ListaPagamentos.FirstOrDefault().Id;

                    foreach (var transacao in item.ListaTransacoes)
                    {
                        var transacaoBD = await _mySqlContext.Transacoes.Where(c => c.Id.Equals(transacao.Id)).AsNoTracking().FirstOrDefaultAsync();

                        transacaoBD.PagId = idPagamento;

                        _mySqlContext.Transacoes.Update(transacaoBD);
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

            foreach(var cliente in clientes)
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

            return listaTransacoesSemOrdem;
        }
    }
}
