export class GestaoPagamentoTransacoesModel {
    Id: number ;
    DtHrLancamento: Date ;
    Descricao: string ;
    Tipo: string ;
    VlBruto: string ;
    VlLiquido: string ;
    CodRef: string ;
    Obs: string ;
    CliId: number ;
    FopId: number ;    
    RceId: number ;
    Grupo: string ;
    UsuNome: string ;
    UsuCpf: string ;
    DtHrAcaoUsuario: Date ;
    DtHrCredito: Date ;
    Status: string ;
    ValorSolicitadoCliente: string ;
    DtHrSolicitacoCliente: Date ;
    DtAgendamento: Date ;
    VlBrutoTransacao: string ;
    QtdParcelaTransacao: string ;
    CodAutorizacaoTransacao: string ;
    NumCartaoTransacao: string ;
    MeioCapturaTransacao: string ;
    TipoOperacaoTransacao: string ;
    ValorLiquidoOperadora: string ;
    ValorTaxaPagSeguro: string ;
    TaxaPagSeguro: string ;
    ValorTaxaPagCliente: string ;
    TaxaPagCliente: string ;
    VlLiquidoCliente: string ;
    NumTerminal: string ;
    EstId: number ;
}