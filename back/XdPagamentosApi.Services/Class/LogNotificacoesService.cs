using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class LogNotificacoesService : BaseService<LogNotificacoes>, ILogNotificacoesService
    {
        public LogNotificacoesService(ILogNotificacoesRepository logNotificacoesRepository) : base(logNotificacoesRepository)
        {

        }
    }
}
