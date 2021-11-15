using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Services.Interfaces
{
    public interface IVwTransacoesSemOrdemPagtoService : IBaseService<VwTransacoesSemOrdemPagto>
    {
        Task<List<TransacoesSemOrdemPagtoPorCliente>> ListaTransacoesSemOrdemPagto(PaginationFilter paginationFilter);

        Task<bool> Gerar(ParamOrdemPagto parametros);
    }
}
