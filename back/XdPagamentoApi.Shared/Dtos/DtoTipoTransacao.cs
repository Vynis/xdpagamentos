using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoTipoTransacao
    {
        public int Id { get; set; }
        public string QtdParcelas { get; set; }
        public string PercDesconto { get; set; }
        public string Status { get; set; }
        public int? CliId { get; set; }
    }
}
