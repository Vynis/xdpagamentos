using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IEstabelecimentoRepository : IBase<Estabelecimento>
    {
        Task<Estabelecimento[]> BuscarComFiltro(PaginationFilter paginationFilter);
        Task<String[]> ExcluirComValidacao(int id);
    }
}
