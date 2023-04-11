using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentoApi.Shared.Enums;

namespace XdPagamentosApi.Domain.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string CnpjCpf { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Fone1 { get; set; }
        public string Fone2 { get; set; }
        public string Email { get; set; }
        public string NumAgencia { get; set; }
        public string NumConta { get; set; }
        public string TipoConta { get; set; }
        public string Status { get; set; }
        public string UltimoAcesso { get; set; }
        public string TipoPessoa { get; set; }
        public string NomeAgrupamento { get; set; }
        public string LimiteCredito { get; set; }
        public TiposChavePix? TipoChavePix { get; set; }
        public string ChavePix { get; set; }


        public int BanId { get; set; }
        public Banco Banco { get; set; }

        public int EstId { get; set; }
        public Estabelecimento Estabelecimento { get; set; }

        public List<RelClienteTerminal> ListaRelClienteTerminal { get; set; }
        public List<TipoTransacao> ListaTipoTransacao { get; set; }
        public List<Pagamentos> ListaPagamentos { get; set; }
        public List<Transacao> ListaTransacoes { get; set; }
        public List<GestaoPagamento> ListaGestaoPagamento { get; set; }

        public List<VwGestaoPagamentoTransacoes> ListaViewGestaoPagamentoTransacoes { get; set; }
    }
}
