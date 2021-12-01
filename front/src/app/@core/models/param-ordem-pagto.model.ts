import { TransacoesSemOrdemPagtoPorClienteModel } from "./transacoes-sem-ordem-pagto-cliente.model";

export class ParamOrdemPagtoModel {
    idConta: number;
    dataLancamentoCredito: Date;
    clientesSelecionados: TransacoesSemOrdemPagtoPorClienteModel[] = [];
}