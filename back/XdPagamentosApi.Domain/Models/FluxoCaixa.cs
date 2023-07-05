using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class FluxoCaixa
    {
        public int Id { get; set; }
        public DateTime DtLancamento { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string TipoPagamento { get; set; }
        public int CorId { get; set; }
        public ContaReceber ContaReceber { get; set; }
        public int CpaId { get; set; }
        public ContaPagar ContaPagar { get; set; }
        public int PcoId { get; set; }
        public PlanoConta PlanoConta { get; set; }
        public int CocId { get; set; }
        public ContaCaixa ContaCaixa { get; set; }
        public DateTime DtCadastro { get; set; }
    }
}
