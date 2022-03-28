using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class TerminalService : BaseService<Terminal>, ITerminalService
    {
        private readonly ITerminalRepository _terminalRepository;

        public TerminalService(ITerminalRepository terminalRepository ) : base(terminalRepository)
        {
            _terminalRepository = terminalRepository;
        }

        public async Task<Terminal[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            return await _terminalRepository.BuscarComFiltro(paginationFilter);
        }

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            return await _terminalRepository.ExcluirComValidacao(id);
        }
    }
}
