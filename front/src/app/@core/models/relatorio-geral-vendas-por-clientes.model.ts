import { GestaoPagamentoTransacoesModel } from "./vw-gestao-pagamento-transacoes.model";

export class RelatorioGeralVendasPorClienteModel {
    nomeCliente: string;
    vlBrutoTotal : string;
    vlTxPagSeguroTotal : string;
    vlTxClienteTotal : string;
    vlLiqOpeTotal : string;
    vlPagtoTotal : string;
    vlLucroTotal : string;
    listaGestaoPagamento: GestaoPagamentoTransacoesModel[] = [];
}