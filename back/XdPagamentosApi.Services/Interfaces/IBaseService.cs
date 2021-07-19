﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XdPagamentosApi.Services.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<bool> Adicionar(TEntity obj);
        Task<bool> Atualizar(TEntity obj);
        Task<bool> Excluir(TEntity obj);
        Task<IEnumerable<TEntity>> ObterTodos();
        Task<TEntity> ObterPorId(int Id);
        Task<IEnumerable<TEntity>> ObterPorDescricao(string Descricao);
        Task<IEnumerable<TEntity>> BuscarExpressao(Expression<Func<TEntity, bool>> predicado);
        Task<bool> AdiconarLista(TEntity[] obj);
        Task<bool> ExcluirLista(TEntity[] obj);
    }
}
