using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class ContaReceberService : BaseService<ContaReceber>, IContaReceberService
    {
        private readonly IContaReceberRepository _repository;

        public ContaReceberService(IContaReceberRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<ContaReceber[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarComFiltro(paginationFilter);  
        }
    }
}
