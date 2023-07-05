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
            service.AddScoped<IContaCaixaRepository, ContaCaixaRepository>();
            service.AddScoped<IOperadoraRepository, OperadoraRepository>();
            service.AddScoped<ISessaoRepository, SessaoRepository>();
            service.AddScoped<ITipoTransacaoRepository, TipoTransacaoRepository>();
            service.AddScoped<ILogNotificacoesRepository, LogNotificacoesRepository>();
            service.AddScoped<IVwTransacoesSemOrdemPagtoRepository, VwTransacoesSemOrdemPagtoRepository>();
            service.AddScoped<IRelContaEstabelecimentoRepository, RelContaEstabelecimentoRepository>();
            service.AddScoped<IGestaoPagamentoRepository, GestaoPagamentoRepository>();
            service.AddScoped<IFormaPagtoRepository, FormaPagtoRepository>();
            service.AddScoped<IRelatoriosRepository, RelatoriosRepository>();
            service.AddScoped<IUsuarioClienteRepository, UsuarioClienteRepository>();
            service.AddScoped<ICentroCustoRepository, CentroCustoRepository>();
            service.AddScoped<IContaPagarRepository, ContaPagarRepository>();
            service.AddScoped<IContaReceberRepository, ContaReceberRepository>();
            service.AddScoped<IPlanoContaRepository, PlanoContaRepository>();
            service.AddScoped<IFluxoCaixaRepository, FluxoCaixaRepository>();
        }
    }
}
