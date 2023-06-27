using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class ContaPagarService : BaseService<ContaPagar>, IContaPagarService
    {
        private readonly IContaPagarRepository _repository;

        public ContaPagarService(IContaPagarRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<ContaPagar[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarComFiltro(paginationFilter);
        }
    }
}
