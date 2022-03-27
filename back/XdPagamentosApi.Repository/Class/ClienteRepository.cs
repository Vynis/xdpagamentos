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
    public class ClienteRepository : Base<Cliente>, IClienteRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;


        public ClienteRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _filtroDinamico = filtroDinamico;
            _mySqlContext = mySqlContext;
        }

        public async Task<Cliente[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
           // await AtualizaTaxas();

            Expression<Func<Cliente, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<Cliente>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.Clientes.Include(x => x.Estabelecimento).ToArrayAsync();

            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(expressionDynamic).Include(x => x.Estabelecimento);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();


            return await query.AsNoTracking().OrderBy(c => c.Nome).ToArrayAsync();
        }

        private async Task AtualizaTaxas()
        {
            var todosClientes = _mySqlContext.Clientes.AsNoTracking().Include(c => c.ListaTipoTransacao).ToList();
            var taxasPadroes = _mySqlContext.TipoTransacoes.Where(x => x.CliId.Equals(0) && x.Status.Equals("A")).AsNoTracking().ToList();

            foreach (var cliente in todosClientes)
            {

                var taxasNaoGravadas = taxasPadroes.ToList().Where(c => !cliente.ListaTipoTransacao.Any(x => x.QtdParcelas == c.QtdParcelas)).ToList();

                if (taxasNaoGravadas.Count() > 0)
                {
                    taxasNaoGravadas.ToList().ForEach(x => x.Id = 0);
                    taxasNaoGravadas.ToList().ForEach(x => x.CliId = cliente.Id);
                    cliente.ListaTipoTransacao.AddRange(taxasNaoGravadas);
                    var retorno = await Atualizar(cliente);

                    if (!retorno)
                        Console.WriteLine(retorno);
                }
            }

        }

        public async override Task<IEnumerable<Cliente>> BuscarExpressao(Expression<Func<Cliente, bool>> predicado)
        {
            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(predicado).AsNoTracking().Include(c => c.ListaTipoTransacao);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async override Task<Cliente> ObterPorId(int Id)
        {
            IQueryable<Cliente> query = _mySqlContext.Clientes.Where(c => c.Id.Equals(Id)).Include(c => c.ListaTipoTransacao);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<Cliente>> ObterTodos()
        {
            IQueryable<Cliente> query = _mySqlContext.Clientes.AsNoTracking().Include(c => c.ListaTipoTransacao);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async override Task<bool> Atualizar(Cliente obj)
        {
            var validaTipoTransacao = _mySqlContext.TipoTransacoes.Where(c => c.CliId.Equals(obj.Id)).AsNoTracking().ToList();

            if (validaTipoTransacao.Count() > 0)
                _mySqlContext.RemoveRange(validaTipoTransacao);

            return await base.Atualizar(obj);
        }

        public async override Task<bool> Excluir(Cliente obj)
        {
            var validaRelClienteTerminais = _mySqlContext.RelClienteTerminais.Where(c => c.CliId.Equals(obj.Id)).AsNoTracking().ToList();

            if (validaRelClienteTerminais.Any())
                return false;

            return await base.Excluir(obj);
        }
    }
}
