using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class CentroCustoService : BaseService<CentroCusto>, ICentroCustoService
    {
        private readonly ICentroCustoRepository _repository;

        public CentroCustoService(ICentroCustoRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            return await _repository.ExcluirComValidacao(id);
        }
    }
}
