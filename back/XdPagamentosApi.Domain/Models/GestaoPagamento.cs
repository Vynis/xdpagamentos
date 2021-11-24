using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class GestaoPagamento
    {
        public int Id { get; set; }
        public DateTime DtHrLancamento { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string VlBruto { get; set; }
        public string VlLiquido { get; set; }
        public string CodRef { get; set; }
        public string Obs { get; set; }
        public int CliId { get; set; }
        public Cliente Cliente { get; set; }
        public int FopId { get; set; }
        public FormaPagto FormaPagto { get; set; }
        public int RceId { get; set; }
        public RelContaEstabelecimento RelContaEstabelecimento { get; set; }
        public string Grupo { get; set; }
        public string UsuNome { get; set; }
        public string UsuCpf { get; set; }
        public DateTime DtHrAcaoUsuario { get; set; }
        public DateTime DtHrCredito { get; set; }
    }
}
