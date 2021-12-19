using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoGestaoPagamento
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
        public DtoCliente Cliente { get; set; }
        public int FopId { get; set; }
        public DtoFormaPagto FormaPagto { get; set; }
        public int RceId { get; set; }
        public DtoRelContaEstabelecimento RelContaEstabelecimento { get; set; }
        public string Grupo { get; set; }
        public string UsuNome { get; set; }
        public string UsuCpf { get; set; }
        public DateTime DtHrAcaoUsuario { get; set; }
        public DateTime? DtHrCredito { get; set; }

        public string Status { get; set; }
        public string ValorSolicitadoCliente { get; set; }
        public DateTime? DtHrSolicitacoCliente { get; set; }

        public string DtHrLancamentoFormatada { 
            get {
                return DtHrLancamento.ToString();
            } 
        }

        public string DtHrCreditoFormatada { 
          get {
                return DtHrCredito.ToString().Equals("01/01/0001 00:00:00") ? "" : DtHrCredito.ToString();
          } 
        }

        public string UsuarioFormatada { 
            get {
                if (string.IsNullOrEmpty(UsuNome) || string.IsNullOrEmpty(UsuCpf))
                    return "";

                var cpf = $"{UsuCpf.Substring(0,3)}.***.***-{UsuCpf.Substring(9, 2)}";
                var usuario = UsuNome.Length > 13 ? $"{UsuNome.Substring(0, 10)}..." : UsuNome;


                return $"{cpf}/{usuario}";
            }
        }

        public string EstabelecimentoFormatada
        {
            get
            {
                if (RelContaEstabelecimento != null)
                    return $"{RelContaEstabelecimento.Estabelecimento.Nome} ({RelContaEstabelecimento.Estabelecimento.CnpjCpf})";
                else
                    return "";
            }
        }
    }
}
