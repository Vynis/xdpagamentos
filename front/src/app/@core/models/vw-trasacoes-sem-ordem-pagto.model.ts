export class VwTransacoesSemOrdemPagtoModel {
    id: number;
    dataOperacao: Date;
    numTerminal: string;
    qtdParcelas: string;
    codTransacao: string;
    vlBruto: string;
    dataGravacao: Date;
    estabelecimento: string;
    clidId: number;
    cliente: string;
    vlLiquido: string;
    vlTxAdmin: string;
    vlTxAdminPercentual: string;
    vlBrutoFormatado: string;
    vlLiquidoFormatado: string;
    vlTaxaAdminFormatado: string;
    dataOperacaoFormatado: string;
    dataGravacaoFormatado: string;
}