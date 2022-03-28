import { PaginationFilterModel } from './../models/configuracao/paginationfilter.model';
import { ClienteModel } from './../models/cliente.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class ClienteService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Cliente`;
  }

  buscar(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-cliente-filtro`, filtro);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }

  inserir(model : ClienteModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : ClienteModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  remover(id : number) {
    return this.http.delete<ModeloBase>(`${this.caminhoApi}/deletar/${id}`);
  }

  alterarSenhaPadrao(id: number) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar-senha-padrao/${id}`, { id });
  }

  agrupar(clientes: ClienteModel[]) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/agrupar`, clientes);
  }

  desagrupar(clientes: ClienteModel[]) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/desagrupar`, clientes);
  }

  buscarGrupo() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/bucar-grupo-clientes`);
  }

}
