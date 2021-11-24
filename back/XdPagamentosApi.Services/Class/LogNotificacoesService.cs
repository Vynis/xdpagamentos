using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Domain.Models.NotificacaoTransacao;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class LogNotificacoesService : BaseService<LogNotificacoes>, ILogNotificacoesService
    {
        private readonly ILogNotificacoesRepository _logNotificacoesRepository;

        public LogNotificacoesService(ILogNotificacoesRepository logNotificacoesRepository) : base(logNotificacoesRepository)
        {
            _logNotificacoesRepository = logNotificacoesRepository;
        }

        public async Task<bool> GerarOrdemPagamento(DtoTransactionPagSeguro dtoTransactionPagSeguro,string estabelecimento)
        {
            return await _logNotificacoesRepository.GerarOrdemPagamento(dtoTransactionPagSeguro, estabelecimento);
        }
    }
}
