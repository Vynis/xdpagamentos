using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class EstabelecimentoService : BaseService<Estabelecimento>, IEstabelecimentoService
    {
        public EstabelecimentoService(IEstabelecimentoRepository estabelecimentoRepository) : base(estabelecimentoRepository)
        {

        }
    }
}
