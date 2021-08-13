import { EstabelecimentoModel } from './estabelecimento.model';
export class TerminalModel {
    id: number;
    numTerminal: string;
    status: string;
    estId: number;
    estabelecimento: EstabelecimentoModel;
}