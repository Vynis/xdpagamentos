export class VwRelatorioSolicitacaoModel {
    id: number;
    dtAgendamento: Date;
    dtHrLancamento: Date;
    dtHrSolicitadoCliente: Date;
    gepDescricao: string;
    valorLiquido: string;
    cliId: number;
    cliNome: string;
    cnpjCpf: string;
    fopId: number;
    fopDescricao: string;
    statusFormatado: string;
}