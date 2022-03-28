using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class ContaCaixaService : BaseService<ContaCaixa>, IContaCaixaService
    {
        private readonly IContaCaixaRepository _contaCaixaRepository;

        public ContaCaixaService(IContaCaixaRepository contaCaixaRepository) : base(contaCaixaRepository)
        {
            _contaCaixaRepository = contaCaixaRepository;
        }

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            return await _contaCaixaRepository.ExcluirComValidacao(id);
        }
    }
}
