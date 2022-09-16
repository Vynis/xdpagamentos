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
    public class TerminalRepository : Base<Terminal>, ITerminalRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public TerminalRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<Terminal[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<Terminal, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<Terminal>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.Terminais.AsNoTracking().Include(c => c.Estabelecimento).Include(c => c.ListaRelClienteTerminal).Include("ListaRelClienteTerminal.Cliente").ToArrayAsync();

            IQueryable<Terminal> query = _mySqlContext.Terminais.Where(expressionDynamic);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().Include(c => c.Estabelecimento).Include(c => c.ListaRelClienteTerminal).Include("ListaRelClienteTerminal.Cliente").ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.NumTerminal).ToArrayAsync();
        }

        public override Task<bool> Atualizar(Terminal obj)
        {

            var relacionamento = _mySqlContext.RelClienteTerminais.AsNoTracking().Where(x => x.TerId == obj.Id).ToArray();

            if (relacionamento.Length > 0)
                _mySqlContext.RemoveRange(relacionamento);

            return base.Atualizar(obj);
        }

        public override async Task<Terminal> ObterPorId(int Id)
        {
            IQueryable<Terminal> query = _mySqlContext.Terminais.Where(x => x.Id.Equals(Id)).Include(x => x.ListaRelClienteTerminal);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<Terminal>> BuscarExpressao(Expression<Func<Terminal, bool>> predicado)
        {
            IQueryable<Terminal> query = _mySqlContext.Terminais.Where(predicado).Include(x => x.ListaRelClienteTerminal);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            var listaErros = new List<string>();

            var terminal = (await _mySqlContext.Terminais.Where(c => c.Id == id).Include(c => c.ListaRelClienteTerminal).ToArrayAsync() ).FirstOrDefault();

            if (terminal == null)
                listaErros.Add("Terminal não encontrado");

            var transacao = await _mySqlContext.Transacoes.Where(c => c.NumTerminal.Equals(terminal.NumTerminal)).ToArrayAsync();
            
            if (transacao.Count() > 0)
                listaErros.Add("Transações");
            
               
            if (listaErros.Count() == 0)
            {
                _mySqlContext.RelClienteTerminais.RemoveRange(terminal.ListaRelClienteTerminal);
                await _mySqlContext.SaveChangesAsync();

                await base.Excluir(terminal);
            }
                

            return listaErros.ToArray();
        }

        public async Task<Terminal[]> BuscaTerminalCliente(int cliId = 0)
        {
            if (cliId == 0)
                return await _mySqlContext.Terminais.AsNoTracking().Include(c => c.Estabelecimento).Include(c => c.ListaRelClienteTerminal).Include("ListaRelClienteTerminal.Cliente").ToArrayAsync();

            var retorno = await _mySqlContext.RelClienteTerminais.Where(x => x.CliId == cliId).Include(c => c.Terminal).Include(c => c.Cliente).ToArrayAsync();

            if (retorno.Count() == 0)
                return new List<Terminal>().ToArray();

            var listaTerminal = new List<Terminal>();

            retorno.ToList().ForEach(x => listaTerminal.Add(x.Terminal));

            return listaTerminal.ToArray();
        }
    }
}
