import { VwTransacoesSemOrdemPagtoModel } from "./vw-trasacoes-sem-ordem-pagto.model";

export class TransacoesSemOrdemPagtoPorTerminalModel {
    idCliente: number;
    NumTerminal: string;
    listaTransacoes: VwTransacoesSemOrdemPagtoModel[] =[];
    vlBrutoTotal: string;
    vlTxAdminTotal: string;
    vlLiquidoTotal: string;
    qtdOperacoes: number;
}