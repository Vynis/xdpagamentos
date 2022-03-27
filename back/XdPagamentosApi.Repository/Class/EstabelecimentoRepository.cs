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
    public class EstabelecimentoRepository: Base<Estabelecimento>, IEstabelecimentoRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public EstabelecimentoRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<Estabelecimento[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<Estabelecimento, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<Estabelecimento>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.Estabelecimentos.AsNoTracking().Include(c => c.Operadora).Include(c => c.ListaRelContaEstabelecimento).Include("ListaRelContaEstabelecimento.ContaCaixa").ToArrayAsync();

            IQueryable<Estabelecimento> query = _mySqlContext.Estabelecimentos.Where(expressionDynamic).Include(c => c.Operadora).Include(c => c.ListaRelContaEstabelecimento).Include("ListaRelContaEstabelecimento.ContaCaixa");

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.Nome).ToArrayAsync();
        }

        public async override Task<IEnumerable<Estabelecimento>> BuscarExpressao(Expression<Func<Estabelecimento, bool>> predicado)
        {
            IQueryable<Estabelecimento> query = _mySqlContext.Estabelecimentos.Where(predicado);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async override Task<Estabelecimento> ObterPorId(int Id)
        {
            IQueryable<Estabelecimento> query = _mySqlContext.Estabelecimentos.Where(c => c.Id.Equals(Id)).Include(c => c.ListaRelContaEstabelecimento);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public override async Task<bool> Atualizar(Estabelecimento obj)
        {
            var buscaRelacionamentoConta =  _mySqlContext.RelContaEstabelecimentos.Where(c => (c.EstId.Equals(obj.Id) && c.CreditoAutomatico.Equals("S")) || ( c.EstId.Equals(obj.Id) && c.CocId.Equals(obj.ListaRelContaEstabelecimento[0].CocId) )).AsNoTracking();

            if (buscaRelacionamentoConta.Count() > 0)
                _mySqlContext.RelContaEstabelecimentos.RemoveRange(buscaRelacionamentoConta);
 
            return await base.Atualizar(obj);
        }

    }
}
