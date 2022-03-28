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
    public class UsuarioRepository : Base<Usuario>, IUsuarioRepository
    {
        private readonly MySqlContext _mySqlContext;

        public UsuarioRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }


        public async override Task<IEnumerable<Usuario>> BuscarExpressao(Expression<Func<Usuario, bool>> predicado)
        {
            IQueryable<Usuario> query = _mySqlContext.Usuarios.Where(predicado).Include(c => c.ListaPermissao).Include("ListaPermissao.Sessao").Include(c => c.ListaUsuarioEstabelecimentos).Include("ListaUsuarioEstabelecimentos.Estabelecimento");

            return await query.AsNoTracking().ToListAsync();
        }

        public async override Task<Usuario> ObterPorId(int Id)
        {
            IQueryable<Usuario> query = _mySqlContext.Usuarios.Where(c => c.Id.Equals(Id)).Include(c => c.ListaPermissao).Include("ListaPermissao.Sessao").Include(c => c.ListaUsuarioEstabelecimentos).Include("ListaUsuarioEstabelecimentos.Estabelecimento"); ;

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async override Task<bool> Atualizar(Usuario obj)
        {
            var permissao = _mySqlContext.Permissoes.Where(c => c.UsuId.Equals(obj.Id)).AsNoTracking().ToList();

            if (permissao.Count() > 0)
                _mySqlContext.RemoveRange(permissao);

            var estabelecimento = _mySqlContext.RelUsuarioEstabelecimentos.Where(c => c.UsuId.Equals(obj.Id)).AsNoTracking().ToList();

            if (estabelecimento.Count() > 0)
                _mySqlContext.RemoveRange(estabelecimento);

            return await base.Atualizar(obj);
        }

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            var listaErros = new List<string>();

            var usuario = (await _mySqlContext.Usuarios.Where(c => c.Id == id).Include(c => c.ListaPermissao).Include(c => c.ListaUsuarioEstabelecimentos).AsNoTracking().ToArrayAsync()).FirstOrDefault();

            if (usuario == null)
                listaErros.Add("Usuario não encontrado");

            if (listaErros.Count() == 0)
            {
                _mySqlContext.Permissoes.RemoveRange(usuario.ListaPermissao);
                await _mySqlContext.SaveChangesAsync();

                _mySqlContext.RelUsuarioEstabelecimentos.RemoveRange(usuario.ListaUsuarioEstabelecimentos);
                await _mySqlContext.SaveChangesAsync();

                await base.Excluir(usuario);
            }

            return listaErros.ToArray();

        }
    }
}
