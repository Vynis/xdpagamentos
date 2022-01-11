using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IVwTransacoesSemOrdemPagtoRepository : IBase<VwTransacoesSemOrdemPagto>
    {
        Task<List<TransacoesSemOrdemPagtoPorCliente>> ListaTransacoesSemOrdemPagto(PaginationFilter paginationFilter);

        Task<List<TransacoesSemOrdemPagtoPorTerminal>> ListaTransacoesSemOrdemPagtoTerminal(PaginationFilter paginationFilter);

        Task<bool> Gerar(ParamOrdemPagto parametros);
    }
}
