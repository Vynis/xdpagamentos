using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Services.Interfaces;

namespace XdPagamentosApi.Services.Class
{
    public class RelContaEstabelecimentoService : BaseService<RelContaEstabelecimento>, IRelContaEstabelecimentoService
    {
        public RelContaEstabelecimentoService(IRelContaEstabelecimentoRepository relContaEstabelecimentoRepository) : base(relContaEstabelecimentoRepository)
        {

        }
    }
}
