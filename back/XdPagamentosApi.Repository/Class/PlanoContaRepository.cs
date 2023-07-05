using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class PlanoContaRepository : Base<PlanoConta>, IPlanoContaRepository
    {
        public PlanoContaRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
        }
    }
}
