using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoPlanoConta
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public string Referencia { get; set; }
        public string Status { get; set; }
    }
}
