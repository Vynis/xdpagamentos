using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class ContaCaixa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }

        public List<RelContaEstabelecimento> ListaRelContaEstabelecimento { get; set; }

        public List<FluxoCaixa> ListaFluxoCaixa { get; set; }
    }
}
