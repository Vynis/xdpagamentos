using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class UsuarioClienteService : BaseService<UsuarioCliente>, IUsuarioClienteService
    {
        public UsuarioClienteService(IUsuarioClienteRepository usuarioClienteRepository) : base(usuarioClienteRepository)
        {

        }
    }
}
