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
    public class RelContaEstabelecimentoRepository : Base<RelContaEstabelecimento>, IRelContaEstabelecimentoRepository
    {
        private readonly MySqlContext _mySqlContext;

        public RelContaEstabelecimentoRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public override async Task<IEnumerable<RelContaEstabelecimento>> BuscarExpressao(Expression<Func<RelContaEstabelecimento, bool>> predicado)
        {
            return await _mySqlContext.RelContaEstabelecimentos.Where(predicado).Include(c => c.Estabelecimento).Include(c => c.ContaCaixa).AsNoTracking().ToListAsync();
        }

        public override async Task<IEnumerable<RelContaEstabelecimento>> ObterTodos()
        {
            return await _mySqlContext.RelContaEstabelecimentos.Include(c => c.Estabelecimento).Include(c => c.ContaCaixa).AsNoTracking().ToListAsync();
        }
    }
}
