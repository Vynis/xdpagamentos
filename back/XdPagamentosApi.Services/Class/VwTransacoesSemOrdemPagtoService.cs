using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class VwTransacoesSemOrdemPagtoService : BaseService<VwTransacoesSemOrdemPagto>, IVwTransacoesSemOrdemPagtoService
    {
        private readonly IVwTransacoesSemOrdemPagtoRepository _vwTransacoesSemOrdemPagtoRepository;

        public VwTransacoesSemOrdemPagtoService(IVwTransacoesSemOrdemPagtoRepository vwTransacoesSemOrdemPagtoRepository) : base(vwTransacoesSemOrdemPagtoRepository)
        {
            _vwTransacoesSemOrdemPagtoRepository = vwTransacoesSemOrdemPagtoRepository;
        }

        public async Task<List<TransacoesSemOrdemPagtoPorCliente>> ListaTransacoesSemOrdemPagto(PaginationFilter paginationFilter)
        {
            return await _vwTransacoesSemOrdemPagtoRepository.ListaTransacoesSemOrdemPagto(paginationFilter);
        }
    }
}
