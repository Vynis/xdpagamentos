﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoRelContaEstabelecimento
    {
        public int Id { get; set; }
        public int CocId { get; set; }
        public DtoContaCaixa ContaCaixa { get; set; }
        public int EstId { get; set; }
        public DtoEstabelecimento Estabelecimento { get; set; }
    }
}
