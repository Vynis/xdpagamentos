import { ClienteModel } from "./cliente.model";

export class RelClienteTerminalModel {
    id: number;
    cliId: number;
    terId: number;
    cliente: ClienteModel;
}