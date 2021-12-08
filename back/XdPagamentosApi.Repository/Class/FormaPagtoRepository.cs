using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class FormaPagtoRepository : Base<FormaPagto>, IFormaPagtoRepository
    {
        public FormaPagtoRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
        }
    }
}
