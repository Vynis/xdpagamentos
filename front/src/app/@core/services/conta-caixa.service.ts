import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ContaCaixaModel } from '../models/contacaixa.model';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class ContaCaixaService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/ContaCaixa`;
  }

  buscarTodos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-todos`);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }

  inserir(model : ContaCaixaModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : ContaCaixaModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  buscarContaCaixaEstabelecimento() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-conta-caixa-estabelecimento`);
  }
}
