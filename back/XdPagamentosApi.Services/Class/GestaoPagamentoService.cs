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

        public async Task<RetGestaoPagamentoTransacoes> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarComFiltro(paginationFilter);
        }

        public async Task<GestaoPagamento[]> BuscarComFiltroCliente(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarComFiltroCliente(paginationFilter);
        }

        public async Task<GestaoPagamento[]> BuscarComFiltroExtrato(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarComFiltroExtrato(paginationFilter);
        }

        public async Task<IEnumerable<GestaoPagamentoPorCliente>> BuscarRelatorioGestaoPagamento(PaginationFilter paginationFilter)
        {
            return await _repository.BuscarRelatorioGestaoPagamento(paginationFilter);
        }

        public async Task<VwRelatorioSaldoCliente> BuscaSaldoCliente(int cliId)
        {
            return await _repository.BuscaSaldoCliente(cliId);
        }
    }
}
