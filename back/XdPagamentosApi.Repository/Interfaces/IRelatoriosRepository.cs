using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IRelatoriosRepository : IBase<object>
    {

        Task<VwRelatorioSolicitacao[]> BuscaRelatorioSolicitacao(PaginationFilter paginationFilter);

        Task<VwRelatorioSaldoCliente[]> BuscaRelatorioSaldoCliente(PaginationFilter paginationFilter);

        Task<VwRelatorioSaldoContaCorrente[]> BuscaRelatorioSaldoContaCorrente(PaginationFilter paginationFilter);

        Task<GraficoVendas> BuscaGraficoVendas(int idCli);

    }
}
