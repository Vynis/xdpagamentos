using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class FluxoCaixaService : BaseService<FluxoCaixa>, IFluxoCaixaService
    {
        private readonly IFluxoCaixaRepository _repository;

        public FluxoCaixaService(IFluxoCaixaRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<bool> AdicionarComBaixa(FluxoCaixa obj, string tipo)
        {
            return await _repository.AdicionarComBaixa(obj, tipo);    
        }

        public async Task<bool> Restaurar(int id, string conta)
        {
            return await _repository.Restaurar(id, conta);
        }
    }
}
