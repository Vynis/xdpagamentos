import { RelContaEstabelecimentoModel } from './rel-conta-estabelecimento.model';
export class EstabelecimentoModel {
    id: number;
    numEstabelecimento: string;
    cnpjCpf: string;
    nome: string;
    endereco: string;
    bairro: string;
    cidade: string;
    estado: string;
    cep: string;
    saldoInicial: string;
    numBanco: string;
    numAgencia: string;
    numConta: string;
    status: string;
    tipo: string;
    opeId: number;
    cocId: number;
    listaRelContaEstabelecimento: RelContaEstabelecimentoModel[] = [];

    constructor() {
        this.estado = 'GO';
        this.status = 'A';
    }

}
