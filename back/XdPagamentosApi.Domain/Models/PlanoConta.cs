using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class PlanoConta
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public string Referencia { get; set; }
        public string Status { get; set; }

        public List<FluxoCaixa> ListaFluxoCaixa { get; set; }
    }
}
