using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class ContaPagar
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string ValorPrevisto { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Status { get; set; }
        public string Obs { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CecId { get; set; }
        public CentroCusto CentroCusto { get; set; }

        public List<FluxoCaixa> ListaFluxoCaixa { get; set; }
    }
}
