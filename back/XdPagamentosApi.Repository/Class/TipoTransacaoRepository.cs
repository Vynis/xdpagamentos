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
    public class TipoTransacaoRepository : Base<TipoTransacao>, ITipoTransacaoRepository
    {
        private readonly MySqlContext _mySqlContext;

        public TipoTransacaoRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }


        public async override Task<IEnumerable<TipoTransacao>> BuscarExpressao(Expression<Func<TipoTransacao, bool>> predicado)
        {

            IQueryable<TipoTransacao> query = _mySqlContext.TipoTransacoes.Where(predicado).AsNoTracking();

            return await query.AsNoTracking().ToArrayAsync();
        }

    }
}
