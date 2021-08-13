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
            else
                return base.ObterTodos().Result.ToArray();

            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(expressionDynamic);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.Nome).ToArrayAsync();
        }

        public async override Task<IEnumerable<Cliente>> BuscarExpressao(Expression<Func<Cliente, bool>> predicado)
        {
            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(predicado);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async override Task<Cliente> ObterPorId(int Id)
        {
            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(c => c.Id.Equals(Id));

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
