import { ModeloBase } from './../models/modelo-balse';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { UsuarioClienteModel } from '../models/usuario-cliente.model';

@Injectable()
export class UsuarioClienteService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/UsuarioCliente`;
  }

  buscar(nome: string = '') {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-usuarios-filtro?nome=${nome}`);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  buscarTodosAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-todos-ativos`);
  }

  inserir(model : UsuarioClienteModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : UsuarioClienteModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  remover(id : number) {
    return this.http.delete<ModeloBase>(`${this.caminhoApi}/deletar/${id}`);
  }
}
