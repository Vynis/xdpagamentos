using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoEstabelecimento
    {
        public int Id { get; set; }
        public string NumEstabelecimento { get; set; }
        public string CnpjCpf { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string SaldoInicial { get; set; }
        public string NumBanco { get; set; }
        public string NumAgencia { get; set; }
        public string NumConta { get; set; }
        public string Status { get; set; }
        public string Tipo { get; set; }

        public int OpeId { get; set; }
        public int CocId { get; set; }

        public string Token { get; set; }
        public string Email { get; set; }

        public List<DtoRelUsuarioEstabelecimento> ListaUsuarioEstabelecimentos { get; set; }

        public List<DtoRelContaEstabelecimento> ListaRelContaEstabelecimento { get; set; }
    }
}
