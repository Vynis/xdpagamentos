using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class TransacoesSemOrdemPagtoPorCliente
    {
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string NumEstabelecimento { get; set; }
        public string NomeEstabelcimento { get; set; }
        public List<VwTransacoesSemOrdemPagto> ListaTransacoes { get; set; }


    }
}
