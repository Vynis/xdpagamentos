using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class GestaoPagamentoService : BaseService<GestaoPagamento>, IGestaoPagamentoService
    {
        private readonly IGestaoPagamentoRepository _repository;

        public GestaoPagamentoService(IGestaoPagamentoRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<GestaoPagamento[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarComFiltro(paginationFilter);
        }
    }
}
