using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.IOC.Services
{
    public static class RegisterServices
    {
        public static void Register(IServiceCollection service)
        {
            service.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            service.AddScoped<IUsuarioService, UsuarioService>();
            service.AddScoped<IClienteService, ClienteService>();
            service.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
            service.AddScoped<IBancoService, BancoService>();
            service.AddScoped<ITerminalService, TerminalService>();
            service.AddScoped<IContaCaixaService, ContaCaixaService>();
            service.AddScoped<IOperadoraService, OperadoraService>();
            service.AddScoped<ISessaoService, SessaoService>();
            service.AddScoped<ITipoTransacaoService, TipoTransacaoService>();
            service.AddScoped<ILogNotificacoesService, LogNotificacoesService>();
            service.AddScoped<IVwTransacoesSemOrdemPagtoService, VwTransacoesSemOrdemPagtoService>();
            service.AddScoped<IRelContaEstabelecimentoService, RelContaEstabelecimentoService>();
            service.AddScoped<IGestaoPagamentoService, GestaoPagamentoService>();
            service.AddScoped<IFormaPagtoService, FormaPagtoService>();
        }
    }
}
