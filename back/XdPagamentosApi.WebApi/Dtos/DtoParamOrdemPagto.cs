﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoParamOrdemPagto
    {
        public int IdConta { get; set; }
        public DateTime DataLancamentoCredito { get; set; }
        public List<DtoTransacoesSemOrdemPagtoPorCliente> ClientesSelecionados { get; set; }
    }
}
