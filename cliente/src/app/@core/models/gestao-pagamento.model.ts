export class GestaoPagamentoModel {
    id: number;
    dtHrLancamento: Date;
    descricao: string;
    tipo: string;
    vlBruto: string;
    vlLiquido: string;
    codRef: string;
    obs: string;
    cliId: number;
    fopId: number;
    rceId: number;
    grupo: string;
    usuNome: string;
    usuCpf: string;
    dtHrAcaoUsuario: Date;
    dtHrCredito: Date;
    status: string;
    valorSolicitadoCliente: string;
    dtAgendamento: Date;

    /**
     *
     */
    constructor() {
        this.id = 0;
    }
}