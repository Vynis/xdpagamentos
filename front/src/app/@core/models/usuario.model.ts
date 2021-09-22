import { PermissaoModel } from "./permissao.model";

export class UsuarioModel {
    id: number;
    nome: string;
    cpf: string;
    senha: string;
    email: string;
    status: string;
    listaPermissao: PermissaoModel[] = [];
}