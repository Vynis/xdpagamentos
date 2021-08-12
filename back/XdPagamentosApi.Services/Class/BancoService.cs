using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class BancoService : BaseService<Banco>, IBancoService
    {
        public BancoService(IBancoRepository bancoRepository) : base(bancoRepository)
        {

        }
    }
}
