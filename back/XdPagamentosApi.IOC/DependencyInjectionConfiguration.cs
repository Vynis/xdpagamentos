using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.IOC.Repository;
using XdPagamentosApi.IOC.Services;

namespace XdPagamentosApi.IOC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection service)
        {
            RegisterServices.Register(service);
            RegisterRepository.Register(service);
        }
    }
}
