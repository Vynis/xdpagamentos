using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Domain.Models.NotificacaoTransacao;
using XdPagamentosApi.PagSeguro.TransferObjects.NotificacaoTransacao;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;

namespace XdPagamentosApi.WebApi
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Usuario, DtoUsuarioLogado>().ReverseMap();
            CreateMap<Usuario, DtoUsuario>().ReverseMap();
            CreateMap<Cliente, DtoCliente>().ReverseMap();
            CreateMap<Estabelecimento, DtoEstabelecimento>().ReverseMap();
            CreateMap<Terminal, DtoTerminal>().ReverseMap();
            CreateMap<RelClienteTerminal, DtoRelClienteTerminal>().ReverseMap();
            CreateMap<ContaCaixa, DtoContaCaixa>().ReverseMap();
            CreateMap<RelContaEstabelecimento, DtoRelContaEstabelecimento>().ReverseMap();
            CreateMap<Sessao, DtoSessao>().ReverseMap();
            CreateMap<Permissao, DtoPermissao>().ReverseMap();
            CreateMap<RelUsuarioEstabelecimento, DtoRelUsuarioEstabelecimento>().ReverseMap();
            CreateMap<TipoTransacao, DtoTipoTransacao>().ReverseMap();
            CreateMap<TransacoesSemOrdemPagtoPorCliente, DtoTransacoesSemOrdemPagtoPorCliente>().ReverseMap();
            CreateMap<VwTransacoesSemOrdemPagto, DtoVwTransacoesSemOrdemPagto>().ReverseMap();
            CreateMap<ParamOrdemPagto, DtoParamOrdemPagto>().ReverseMap();
            CreateMap<DtoTransactionPagSeguro, TransactionPagSeguro>().ReverseMap();
            CreateMap<DtoCreditorFees, CreditorFees>().ReverseMap();
            CreateMap<DtoDeviceInfo, DeviceInfo>().ReverseMap();
            CreateMap<DtoItem, Item>().ReverseMap();
            CreateMap<DtoItems, Items>().ReverseMap();
            CreateMap<DtoPaymentMethod, PaymentMethod>().ReverseMap();
            CreateMap<DtoPrimaryReceiver, PrimaryReceiver>().ReverseMap();
            CreateMap<DtoGestaoPagamento, GestaoPagamento>().ReverseMap();
            CreateMap<DtoTransacoesSemOrdemPagtoPorTerminal, TransacoesSemOrdemPagtoPorTerminal>().ReverseMap();
            CreateMap<DtoClienteLista, Cliente>().ReverseMap();
            CreateMap<DtoTerminalLista, Terminal>().ReverseMap();
            CreateMap<DtoOperadora, Operadora>().ReverseMap();
            CreateMap<DtoEstabelecimentoLista, Estabelecimento>().ReverseMap();
            CreateMap<DtoVwGestaoPagamentoTransacoes, VwGestaoPagamentoTransacoes>().ReverseMap();
            CreateMap<DtoRetornoGestaoPagamento, RetGestaoPagamentoTransacoes>().ReverseMap();


            CreateMap<Cliente, DtoUsuarioLogado>().ForMember(x => x.Cpf, c => c.MapFrom(s => s.CnpjCpf)).ReverseMap();
        }
    }
}
