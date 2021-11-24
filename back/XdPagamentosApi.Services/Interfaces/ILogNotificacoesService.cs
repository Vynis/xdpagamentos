using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Domain.Models.NotificacaoTransacao;

namespace XdPagamentosApi.Services.Interfaces
{
    public interface ILogNotificacoesService : IBaseService<LogNotificacoes>
    {
        Task<bool> GerarOrdemPagamento(DtoTransactionPagSeguro dtoTransactionPagSeguro,string estabelecimento);
    }
}
