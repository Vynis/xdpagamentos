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
    public class UsuarioClienteRepository : Base<UsuarioCliente>, IUsuarioClienteRepository
    {
        private readonly MySqlContext _mySqlContext;

        public UsuarioClienteRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public async override Task<IEnumerable<UsuarioCliente>> BuscarExpressao(Expression<Func<UsuarioCliente, bool>> predicado)
        {
            IQueryable<UsuarioCliente> query = _mySqlContext.UsuarioClientes.Where(predicado);

            return await query.AsNoTracking().ToListAsync();
        }

        public async override Task<UsuarioCliente> ObterPorId(int Id)
        {
            IQueryable<UsuarioCliente> query = _mySqlContext.UsuarioClientes.Where(c => c.Id.Equals(Id)); 

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async override Task<bool> Excluir(UsuarioCliente obj)
        {

            var valida = await  _mySqlContext.Clientes.Where(c => c.UscId == obj.Id).ToListAsync();

            if (valida.Count() > 0)
                return false;

            return await base.Excluir(obj);
        }
    }
}
