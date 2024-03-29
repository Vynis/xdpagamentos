﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IContaReceberRepository : IBase<ContaReceber>
    {
        Task<ContaReceber[]> BuscarComFiltro(PaginationFilter paginationFilter);
    }
}
