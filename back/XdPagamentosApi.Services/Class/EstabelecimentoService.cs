using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class EstabelecimentoService : BaseService<Estabelecimento>, IEstabelecimentoService
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepository;

        public EstabelecimentoService(IEstabelecimentoRepository estabelecimentoRepository) : base(estabelecimentoRepository)
        {
            _estabelecimentoRepository = estabelecimentoRepository;
        }

        public async Task<Estabelecimento[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            return await _estabelecimentoRepository.BuscarComFiltro(paginationFilter);
        }

    }
}
