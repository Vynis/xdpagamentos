﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface ITerminalRepository : IBase<Terminal>
    {
        Task<Terminal[]> BuscarComFiltro(PaginationFilter paginationFilter);

        Task<Terminal[]> BuscaTerminalCliente(int cliId = 0);

        Task<String[]> ExcluirComValidacao(int id);
    }
}
