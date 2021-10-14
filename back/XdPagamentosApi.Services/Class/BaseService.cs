using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IBase<TEntity> _repository;

        public BaseService(IBase<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Adicionar(TEntity obj)
        {
            return await _repository.Adicionar(obj);
        }

        public async Task<bool> AdiconarLista(TEntity[] obj)
        {
            return await _repository.AdiconarLista(obj);
        }

        public async Task<bool> Atualizar(TEntity obj)
        {
            return await _repository.Atualizar(obj);
        }

        public async Task<bool> AtualizarLista(List<TEntity> obj)
        {
            return await _repository.AtualizarLista(obj);
        }

        public async Task<IEnumerable<TEntity>> BuscarExpressao(Expression<Func<TEntity, bool>> predicado)
        {
            return await _repository.BuscarExpressao(predicado);
        }

        public async Task<bool> Excluir(TEntity obj)
        {
            return await _repository.Excluir(obj);
        }

        public async Task<bool> ExcluirLista(TEntity[] obj)
        {
            return await _repository.ExcluirLista(obj);
        }

        public async Task<IEnumerable<TEntity>> ObterPorDescricao(string Descricao)
        {
            return await _repository.ObterPorDescricao(Descricao);
        }

        public async Task<TEntity> ObterPorId(int Id)
        {
            return await _repository.ObterPorId(Id);
        }

        public async Task<IEnumerable<TEntity>> ObterTodos()
        {
            return await _repository.ObterTodos();
        }
    }
}
