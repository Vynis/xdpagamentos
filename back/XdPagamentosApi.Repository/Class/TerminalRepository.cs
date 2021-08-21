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
    public class TerminalRepository : Base<Terminal>, ITerminalRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public TerminalRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<Terminal[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<Terminal, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<Terminal>(paginationFilter.Filtro.ToList());
            else
                return base.ObterTodos().Result.ToArray();

            IQueryable<Terminal> query = _mySqlContext.Terminais.Where(expressionDynamic);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.NumTerminal).ToArrayAsync();
        }

        public override Task<bool> Atualizar(Terminal obj)
        {

            var relacionamento = _mySqlContext.RelClienteTerminais.AsNoTracking().Where(x => x.TerId == obj.Id).ToArray();

            if (relacionamento.Length > 0)
                _mySqlContext.RemoveRange(relacionamento);

            return base.Atualizar(obj);
        }

        public override async Task<Terminal> ObterPorId(int Id)
        {
            IQueryable<Terminal> query = _mySqlContext.Terminais.Where(x => x.Id.Equals(Id)).Include(x => x.ListaRelClienteTerminal);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<Terminal>> BuscarExpressao(Expression<Func<Terminal, bool>> predicado)
        {
            IQueryable<Terminal> query = _mySqlContext.Terminais.Where(predicado).Include(x => x.ListaRelClienteTerminal);

            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}
