using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Repository.Class;
using XdPagamentosApi.Repository.Interfaces;

namespace XdPagamentosApi.IOC.Repository
{
    public static class RegisterRepository
    {
        public static void Register(IServiceCollection service)
        {
            service.AddScoped(typeof(IBase<>), typeof(Base<>));
            service.AddScoped<IUsuarioRepository, UsuarioRepository>();
        }
    }
}
