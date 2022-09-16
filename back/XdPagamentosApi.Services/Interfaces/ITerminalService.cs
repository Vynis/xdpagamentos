using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Services.Interfaces
{
    public interface ITerminalService : IBaseService<Terminal>
    {
        Task<Terminal[]> BuscarComFiltro(PaginationFilter paginationFilter);

        Task<Terminal[]> BuscaTerminalCliente(int cliId = 0);

        Task<String[]> ExcluirComValidacao(int id);
    }
}
