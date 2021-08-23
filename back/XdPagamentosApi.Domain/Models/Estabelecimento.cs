using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Estabelecimento
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

        public List<Cliente> ListaClientes { get; set; }
        public List<Terminal> ListaTerminais { get; set; }
        public List<RelContaEstabelecimento> ListaRelContaEstabelecimento { get; set; }
    }
}
