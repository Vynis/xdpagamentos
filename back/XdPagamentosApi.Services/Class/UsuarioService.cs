using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class UsuarioService : BaseService<Usuario>, IUsuarioService
    {
        public UsuarioService(IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
        }
    }
}
