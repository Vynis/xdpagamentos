using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoClienteLista
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CnpjCpf { get; set; }
        public DtoEstabelecimento Estabelecimento { get; set; }
        public string LimiteCredito { get; set; }
        public string Status { get; set; }
        public string TipoPessoa { get; set; }
        public string NomeAgrupamento { get; set; }

        public string EstabelecimentoFormatado
        {
            get
            {
                return Estabelecimento.Nome;
            }
        }

        public string CnpjCpfFormatado
        {
            get
            {
                return TipoPessoa.Equals("PF") ? $"CPF - {CnpjCpf}" : $"CNPJ - {CnpjCpf}";
            }
        } 
    }
}
