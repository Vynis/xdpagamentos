using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Services.Interfaces
{
    public interface IContaCaixaService : IBaseService<ContaCaixa>
    {
        Task<String[]> ExcluirComValidacao(int id);
    }
}
