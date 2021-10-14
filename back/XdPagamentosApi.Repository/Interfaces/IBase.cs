using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XdPagamentosApi.Repository.Interfaces
{
    public interface IBase<TEntity> where TEntity : class
    {
        Task<bool> Adicionar(TEntity obj);
        Task<bool> Atualizar(TEntity obj);
        Task<bool> AtualizarLista(List<TEntity> obj);
        Task<bool> SaveChangesAsync();
        Task<bool> Excluir(TEntity obj);
        Task<IEnumerable<TEntity>> ObterTodos();
        Task<TEntity> ObterPorId(int Id);
        Task<IEnumerable<TEntity>> ObterPorDescricao(string Descricao);
        Task<IEnumerable<TEntity>> BuscarExpressao(Expression<Func<TEntity, bool>> predicado);
        Task<bool> AdiconarLista(TEntity[] obj);
        Task<bool> ExcluirLista(TEntity[] obj);
    }
}
