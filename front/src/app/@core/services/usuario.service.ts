import { UsuarioSenhaModel } from './../models/usuario-senha.model';
import { UsuarioModel } from './../models/usuario.model';
import { ModeloBase } from './../models/modelo-balse';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable()
export class UsuarioService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Usuario`;
  }

  buscar(nome: string = '') {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-usuarios-filtro?nome=${nome}`);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  inserir(model : UsuarioModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : UsuarioModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  alterarSenha(usuarioSenha: UsuarioSenhaModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar-senha`, usuarioSenha);
  }

  remover(id : number) {
    return this.http.delete<ModeloBase>(`${this.caminhoApi}/deletar/${id}`);
  }
}
