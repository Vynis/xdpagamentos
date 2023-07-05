using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtContaReceber
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string Obs { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CecId { get; set; }
        public DtoCentroCusto CentroCusto { get; set; }

        public string DtEmissao
        {
            get
            {
                return DataCadastro.ToString();
            }
        }

        public string DtVencimento
        {
            get
            {
                return Data.ToString();
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
                return "";
            }
        }


    }
}
