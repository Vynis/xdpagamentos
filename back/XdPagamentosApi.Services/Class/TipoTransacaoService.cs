using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class TipoTransacaoService : BaseService<TipoTransacao>, ITipoTransacaoService
    {
        public TipoTransacaoService(ITipoTransacaoRepository tipoTransacaoRepository) : base(tipoTransacaoRepository)
        {

        }
    }
}
