import { SessaoModel } from './sessao.model';
export class PermissaoModel {
    id: number;
    sesId: number;
    sessao: SessaoModel;
    usuId: number;
}