using System;
using System.Globalization;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoVwGestaoPagamentoTransacoes
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
        public DateTime? DtHrAcaoUsuario { get; set; }
        public DateTime DtHrCredito { get; set; }
        public string Status { get; set; }
        public string ValorSolicitadoCliente { get; set; }
        public DateTime? DtHrSolicitacoCliente { get; set; }
        public DateTime? DtAgendamento { get; set; }
        public string VlBrutoTransacao { get; set; }
        public string QtdParcelaTransacao { get; set; }
        public string CodAutorizacaoTransacao { get; set; }
        public string NumCartaoTransacao { get; set; }
        public string MeioCapturaTransacao { get; set; }
        public string TipoOperacaoTransacao { get; set; }
        public string ValorLiquidoOperadora { get; set; }
        public string ValorTaxaPagSeguro { get; set; }
        public string TaxaPagSeguro { get; set; }
        public string ValorTaxaPagCliente { get; set; }
        public string TaxaPagCliente { get; set; }
        public string VlLiquidoCliente { get; set; }
        public string NumTerminal { get; set; }

        public int EstId { get; set; }
        public string Estabelecimento { get; set; }

        public string ValorPercentualTaxaPagSeguro { get; set; }
        public string ValorPercentualTaxaPagCliente { get; set; }

        public string DtHrLancamentoFormatada
        {
            get
            {
                return DtHrLancamento.ToString();
            }
        }

        public string DtHrCreditoFormatada
        {
            get
            {
                return DtHrCredito.ToString().Equals("01/01/0001 00:00:00") ? "" : DtHrCredito.ToString();
            }
        }

        public string UsuarioFormatada
        {
            get
            {
                if (string.IsNullOrEmpty(UsuNome) || string.IsNullOrEmpty(UsuCpf) || UsuNome.Equals("-") || UsuCpf.Equals("-"))
                    return "";

                var cpf = $"{UsuCpf.Substring(0, 3)}.***.***-{UsuCpf.Substring(9, 2)}";
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

        public string TipoFormatado
        {
            get
            {
                return string.IsNullOrEmpty(Tipo) ? "" : Tipo.Equals("C") ? "C - Crédito" : "D - Débito";
            }
        }

        public string ValorFormatado
        {
            get
            {
                return string.IsNullOrEmpty(VlLiquidoCliente) || VlLiquidoCliente.Equals("0,00") ? string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", ValorSolicitadoCliente) : string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", VlLiquidoCliente);
            }
        }

        public string ValorBrutoFormatado
        {
            get
            {
                if (VlBruto == null)
                    return "0,00";

                return VlBruto.ToString(CultureInfo.GetCultureInfo("pt-BR"));
            }
        }

        public string ValorLucroFormatado
        {
            get
            {
                if (!CodRef.Contains("ORPID"))
                    return "0,00";

                var vlLiquidoOperado = decimal.Parse(ValorLiquidoOperadora.Trim().Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });
                var vlLiquidoLucro = decimal.Parse(VlLiquidoCliente.Trim().Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });

                return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", vlLiquidoOperado - vlLiquidoLucro);


            }
        }

        public string StatusFormatado
        {
            get
            {
                switch (Status)
                {
                    case "AP":
                        return "AP - APROVADO";
                    case "PE":
                        return "PE - PENDENTE";
                    case "CA":
                        return "CA - CANCELADO";
                    default:
                        return "";
                }
            }
        }

    }
}
