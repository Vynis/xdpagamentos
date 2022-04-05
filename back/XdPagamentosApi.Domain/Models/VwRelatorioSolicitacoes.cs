using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class VwRelatorioSolicitacao
    {
        public int Id { get; set; }
        public DateTime? DtAgendamento { get; set; }
        public DateTime DtHrLancamento { get; set; }
        public DateTime? DtHrSolicitadoCliente { get; set; }
        public string GepDescricao { get; set; }
        public string ValorLiquido { get; set; }
        public int CliId { get; set; }
        public string CliNome { get; set; }
        public string CnpjCpf { get; set; }
        public int FopId { get; set; }
        public string FopDescricao { get; set; }
        public string StatusFormatado { get; set; }
    }
}
