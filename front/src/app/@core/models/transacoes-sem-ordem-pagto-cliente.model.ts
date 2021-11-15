import { VwTransacoesSemOrdemPagtoModel } from "./vw-trasacoes-sem-ordem-pagto.model";

export class TransacoesSemOrdemPagtoPorClienteModel {
    idCliente: number;
    nomeCliente: string;
    numEstabelecimento: string;
    nomeEstabelcimento: string;
    listaTransacoes: VwTransacoesSemOrdemPagtoModel[] =[];
    vlBrutoTotal: string;
    vlTxAdminTotal: string;
    vlLiquidoTotal: string;
    qtdOperacoes: number;
}