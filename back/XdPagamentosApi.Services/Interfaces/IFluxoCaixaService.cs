using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Services.Interfaces
{
    public interface IFluxoCaixaService : IBaseService<FluxoCaixa>
    {
        Task<bool> AdicionarComBaixa(FluxoCaixa obj, string tipo);

        Task<bool> Restaurar(int id, string conta);
    }
}
