import { CentroCustoModel } from "./centro-custo.model";

export class ContaPagarModel {
    id: number;
    descricao: string;
    valor: string;
    data: Date;
    status: string;
    obs: string;
    dataCadastro: Date;
    cecId: number;
    centroCusto: CentroCustoModel;
}