using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class Base<TEntity> : IBase<TEntity> where TEntity : class
    {
        protected readonly MySqlContext _mySqlContext;
        public Base(MySqlContext mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public virtual async Task<bool> AdiconarLista(TEntity[] obj)
        {
            _mySqlContext.AddRange(obj);
            return await _mySqlContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Adicionar(TEntity obj)
        {
            _mySqlContext.Add(obj);
            return await _mySqlContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Atualizar(TEntity obj)
        {
            _mySqlContext.Update(obj);
            return await _mySqlContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Excluir(TEntity obj)
        {
            _mySqlContext.Remove(obj);
            return await _mySqlContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<IEnumerable<TEntity>> BuscarExpressao(Expression<Func<TEntity, bool>> predicado)
        {
            return await _mySqlContext.Set<TEntity>().Where(predicado).ToArrayAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> ObterPorDescricao(string Descricao)
        {
            return await BuscarExpressao(b => b.GetType().Name.Contains(Descricao));
        }

        public virtual async Task<TEntity> ObterPorId(int Id)
        {
            return await _mySqlContext.Set<TEntity>().FindAsync(Id);
        }

        public async virtual Task<IEnumerable<TEntity>> ObterTodos()
        {
            return await _mySqlContext.Set<TEntity>().ToArrayAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _mySqlContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> ExcluirLista(TEntity[] obj)
        {
            _mySqlContext.RemoveRange(obj);
            return await _mySqlContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> AtualizarLista(List<TEntity> obj)
        {
            _mySqlContext.UpdateRange(obj);
            return await _mySqlContext.SaveChangesAsync() > 0;
        }
    }
}
