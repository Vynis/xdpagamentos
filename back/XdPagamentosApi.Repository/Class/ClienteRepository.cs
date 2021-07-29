using FiltrDinamico.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class ClienteRepository : Base<Cliente>, IClienteRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;


        public ClienteRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _filtroDinamico = filtroDinamico;
            _mySqlContext = mySqlContext;
        }

        public async Task<Cliente[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<Cliente, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<Cliente>(paginationFilter.Filtro.ToList());

            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(expressionDynamic).Include(c => c.Banco).Include(c => c.Estabelecimento);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.Nome).ToArrayAsync();
        }
    }
}
