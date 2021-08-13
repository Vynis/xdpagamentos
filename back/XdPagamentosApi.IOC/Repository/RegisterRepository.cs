using FiltrDinamico.Core;
using FiltrDinamico.Core.Interpreters;
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
            service.AddScoped<IFiltroDinamico, FiltroDinamico>();
            service.AddScoped<IFilterInterpreterFactory, FilterInterpreterFactory>();
            service.AddScoped<IUsuarioRepository, UsuarioRepository>();
            service.AddScoped<IClienteRepository, ClienteRepository>();
            service.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();
            service.AddScoped<IBancoRepository, BancoRepository>();
            service.AddScoped<ITerminalRepository, TerminalRepository>();
        }
    }
}
