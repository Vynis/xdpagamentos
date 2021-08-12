using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class ClienteService : BaseService<Cliente>, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository) : base(clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            if (paginationFilter.Filtro.Count() == 0)
                return _clienteRepository.ObterTodos().Result.ToArray();

            return await _clienteRepository.BuscarComFiltro(paginationFilter);
        }
    }
}
