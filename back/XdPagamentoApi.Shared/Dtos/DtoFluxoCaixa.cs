using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentoApi.Shared.Dtos
{
    public  class DtoFluxoCaixa
    {
        public int Id { get; set; }
        public DateTime DtLancamento { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string TipoPagamento { get; set; }
        public int CorId { get; set; }
        public DtoContaReceber ContaReceber { get; set; }
        public int CpaId { get; set; }
        public DtoContaPagar ContaPagar { get; set; }
        public int PcoId { get; set; }
        public DtoPlanoConta PlanoConta { get; set; }
        public int CocId { get; set; }
        public DtoContaCaixa ContaCaixa { get; set; }
        public DateTime DtCadastro { get; set; }
    }
}
