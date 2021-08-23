import { RelContaEstabelecimentoModel } from './rel-conta-estabelecimento.model';
export class ContaCaixaModel {
    id: number;
    descricao: string;
    status: string;
    listaRelContaEstabelecimento: RelContaEstabelecimentoModel[] = [];

    constructor() {
        this.status = 'A';
    }
}