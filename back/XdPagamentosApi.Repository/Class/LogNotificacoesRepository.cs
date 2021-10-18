using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class LogNotificacoesRepository : Base<LogNotificacoes>, ILogNotificacoesRepository
    {
        public LogNotificacoesRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {

        }
    }
}
