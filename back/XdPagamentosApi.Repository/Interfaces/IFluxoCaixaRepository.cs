using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IFluxoCaixaRepository : IBase<FluxoCaixa>
    {
        Task<bool> AdicionarComBaixa(FluxoCaixa obj, string tipo);

        Task<bool> Restaurar(int id,string conta);
    }
}
