import { ClienteModel } from "./cliente.model";

export class TipoTransacaoModel {
    id: number;
    qtdParcelas: string;
    percDesconto: string;
    status: string;
    cliId: number;
}