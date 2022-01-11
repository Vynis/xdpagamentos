import { TransacoesSemOrdemPagtoPorTerminalModel } from './transacoes-sem-ordem-pagto-terminal.model';
import { TransacoesSemOrdemPagtoPorClienteModel } from "./transacoes-sem-ordem-pagto-cliente.model";

export class ParamOrdemPagtoModel {
    idConta: number;
    idCliente: number;
    dataLancamentoCredito: Date;
    clientesSelecionados: TransacoesSemOrdemPagtoPorClienteModel[] = [];
    terminaisSelecionados: TransacoesSemOrdemPagtoPorTerminalModel[] =[];
}