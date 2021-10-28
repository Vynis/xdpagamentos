import { EstabelecimentoModel } from "./estabelecimento.model";
import { UsuarioModel } from "./usuario.model";

export class RelUsuarioEstabelecimentoModel {
    id: number;
    estId: number;
    estabelecimento: EstabelecimentoModel;
    usuId: number;
}