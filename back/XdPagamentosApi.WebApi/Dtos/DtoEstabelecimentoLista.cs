using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoEstabelecimentoLista
    {
        public int Id { get; set; }
        public string NumEstabelecimento { get; set; }
        public string CnpjCpf { get; set; }
        public string Nome { get; set; }
        public int OpeId { get; set; }
        public DtoOperadora Operadora { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public List<DtoRelContaEstabelecimento> ListaRelContaEstabelecimento { get; set; }

        public string OperadoraFormatado { 
            get
            {
                return Operadora.Nome;
            }
        }

        public string ContaFormatado { 
            get
            {
                return ListaRelContaEstabelecimento.Count() > 0 ? ListaRelContaEstabelecimento.FirstOrDefault().ContaCaixa.Descricao : "-";
            } 
        }

        //public string CnpjCpfFormatado { 
        //    get
        //    {
        //        return CnpjCpf.Length > 11 ? $"CNPJ - {CnpjCpf}" : $"CPF - {CnpjCpf}";
        //    }
        //}
    }
}
