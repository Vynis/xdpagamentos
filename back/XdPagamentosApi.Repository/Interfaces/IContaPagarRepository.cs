﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IContaPagarRepository : IBase<ContaPagar>
    {
        Task<ContaPagar[]> BuscarComFiltro(PaginationFilter paginationFilter);
    }
}
