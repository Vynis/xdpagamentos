export class ResumoLancamentosModel {    
    cliente: string;
    periodo: string;
    saldoAnterior: string;
    entradas: string;
    saidas: string;
    saldoAtual: string;

    contaCaixa: string;
    estabelecimento: string;

    /**
     *
     */
    constructor() {
        this.cliente = "-";
        this.periodo = "-";
        this.saldoAnterior = "-";
        this.entradas = "-";
        this.saidas = "-";
        this.saldoAtual = "-";
    }
}