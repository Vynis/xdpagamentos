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
    public class ContaCaixaRepository : Base<ContaCaixa>, IContaCaixaRepository
    {
        private readonly MySqlContext _mySqlContext;

        public ContaCaixaRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public override Task<bool> Atualizar(ContaCaixa obj)
        {
            var relacionamento = _mySqlContext.RelContaEstabelecimentos.AsNoTracking().Where(c => c.CocId.Equals(obj.Id)).ToArray();

            if (relacionamento.Length > 0)
                _mySqlContext.RemoveRange(relacionamento);

            return base.Atualizar(obj);
        }

        public override async Task<ContaCaixa> ObterPorId(int Id)
        {
            IQueryable<ContaCaixa> query = _mySqlContext.ContaCaixas.Where(c => c.Id.Equals(Id)).Include(c => c.ListaRelContaEstabelecimento).Include("ListaRelContaEstabelecimento.Estabelecimento");

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<ContaCaixa>> BuscarExpressao(Expression<Func<ContaCaixa, bool>> predicado)
        {

            IQueryable<ContaCaixa> query = _mySqlContext.ContaCaixas.Where(predicado).Include(c => c.ListaRelContaEstabelecimento);

            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}
