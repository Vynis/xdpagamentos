import { BancoModel } from "./banco.model";
import { EstabelecimentoModel } from "./estabelecimento.model";

export class ClienteModel {
    id: number ;
    nome: string ;
    senha: string ;
    cnpjCpf: string ;
    endereco: string ;
    bairro: string ;
    cidade: string ;
    estado: string ;
    cep: string ;
    fone1: string ;
    fone2: string ;
    email: string ;
    numAgencia: string ;
    numConta: string ;
    tipoConta: string ;
    status: string ;
    ultimoAcesso: string ;
    banId: number ;
    banco: BancoModel ;
    estId: number ;
    estabelecimento: EstabelecimentoModel ;
    tipoPessoa: string;

    /**
     *
     */
    constructor() {
        this.tipoPessoa = 'PF';
        this.estado = 'GO';
    }
}