using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class RelatoriosService : BaseService<object>, IRelatoriosService
    {
        private readonly IRelatoriosRepository _relatoriosRepository;

        public RelatoriosService(IRelatoriosRepository relatoriosRepository) : base(relatoriosRepository)
        {
            _relatoriosRepository = relatoriosRepository;
        }

        public async Task<VwRelatorioSaldoCliente[]> BuscaRelatorioSaldoCliente(PaginationFilter paginationFilter)
        {
            return await _relatoriosRepository.BuscaRelatorioSaldoCliente(paginationFilter);
        }

        public async Task<VwRelatorioSolicitacao[]> BuscaRelatorioSolicitacao(PaginationFilter paginationFilter)
        {
            return await _relatoriosRepository.BuscaRelatorioSolicitacao(paginationFilter);
        }
    }
}
