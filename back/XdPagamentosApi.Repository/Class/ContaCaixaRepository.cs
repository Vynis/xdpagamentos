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

        public override async Task<bool> Atualizar(ContaCaixa obj)
        {
            var relacionamento = _mySqlContext.RelContaEstabelecimentos.AsNoTracking().Where(c => c.CocId.Equals(obj.Id)).ToArray();

            var listaNova = new List<RelContaEstabelecimento>();

            foreach(var item in relacionamento)
            {
                var existeRegistro = obj.ListaRelContaEstabelecimento.Where(c => c.EstId == item.EstId).ToList().FirstOrDefault();

                if (existeRegistro != null)
                {
                    obj.ListaRelContaEstabelecimento.Where(c =>  c.EstId == item.EstId).ToList().ForEach(c => c.Id = item.Id);
                } 
                else
                {
                    _mySqlContext.RelContaEstabelecimentos.Remove(item);
                    await _mySqlContext.SaveChangesAsync();
                }
            }


            return await base.Atualizar(obj);
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

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            var listaErros = new List<string>();

            var contaCaixa = (await _mySqlContext.ContaCaixas.Where(c => c.Id == id).ToArrayAsync()).FirstOrDefault();

            if (contaCaixa == null)
                listaErros.Add("Conta Caixa não encontrado");

            var relContaEstabelecimento = await _mySqlContext.RelContaEstabelecimentos.Where(c => c.CocId == id).Include(c => c.ListaGestaoPagamento).ToArrayAsync();

            foreach( var item in relContaEstabelecimento)
                if (item.ListaGestaoPagamento.Count() > 0)
                    listaErros.Add($"Gestão de Pagamento (Estabelecimento: {item.EstId} | Qtd: {item.ListaGestaoPagamento.Count()})");
  
            if (listaErros.Count() == 0)
            {
                _mySqlContext.RelContaEstabelecimentos.RemoveRange(relContaEstabelecimento);
                await _mySqlContext.SaveChangesAsync();

                await base.Excluir(contaCaixa);
            }

            return listaErros.ToArray();
        }
    }
}
