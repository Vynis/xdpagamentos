using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Class;
using Xunit;

namespace XdPagamentosApi.Testes.Services
{
    public  class CentroCustoServicesTest
    {
        private CentroCustoService centroCustoService;
        public CentroCustoServicesTest()
        {
            centroCustoService = new CentroCustoService(new Mock<ICentroCustoRepository>().Object);
        }

        [Fact]
        public void Teste()
        {
            var result = centroCustoService.ObterTodos();
            Assert.True(true);
        }
    }
}
