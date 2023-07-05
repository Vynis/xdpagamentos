using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoContaReceber
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
        public DtoCentroCusto CentroCusto { get; set; }

        public List<DtoFluxoCaixa> ListaFluxoCaixa { get; set; }

        public string DtEmissaoFormatada
        {
            get
            {
                return DataEmissao == null ? "" : DataEmissao.ToString("dd/MM/yyyy");
            }
        }

        public string DtVencimentoFormatada
        {
            get
            {
                return DataVencimento == null ? "" : DataVencimento.ToString("dd/MM/yyyy");
            }
        }

        public string DescricaoCentroCusto
        {
            get
            {
                return CentroCusto.Descricao;
            }
        }

        public string FluxoCaixa
        {
            get
            {
                var valorReal = FormataValorDecimal(Valor);
                var valorPrevisto = FormataValorDecimal(ValorPrevisto);
                decimal valorPago = 0;

                ListaFluxoCaixa.ForEach(x => valorPago += FormataValorDecimal(x.Valor));

                var valorRestante = Status.Equals("PG") ? 0 : valorReal - valorPago;


                return $"PV: {ValorMoedaBRDecimal(valorPrevisto)}  - RE: {ValorMoedaBRDecimal(valorReal)}  - PG: {ValorMoedaBRDecimal(valorPago)}  - DV: {ValorMoedaBRDecimal(valorRestante)}";
            }
        }


        private decimal FormataValorDecimal(string valor) => decimal.Parse(valor.Trim().Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });

        private string ValorMoedaBRDecimal(decimal valor) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", valor);


    }
}
