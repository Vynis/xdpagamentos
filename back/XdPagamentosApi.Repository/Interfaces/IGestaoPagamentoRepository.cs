using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IGestaoPagamentoRepository : IBase<GestaoPagamento>
    {
        Task<GestaoPagamento[]> BuscarComFiltro(PaginationFilter paginationFilter);

        Task<GestaoPagamento[]> BuscarComFiltroCliente(PaginationFilter paginationFilter);
    }
}
