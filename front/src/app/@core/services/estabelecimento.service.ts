import { EstabelecimentoModel } from './../models/estabelecimento.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { PaginationFilterModel } from '../models/configuracao/paginationfilter.model';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class EstabelecimentoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Estabelecimento`;
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }

  buscar(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-por-filtro`, filtro);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  inserir(model : EstabelecimentoModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : EstabelecimentoModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }
  
}