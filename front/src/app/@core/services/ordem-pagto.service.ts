import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { PaginationFilterModel } from '../models/configuracao/paginationfilter.model';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class OrdemPagtoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/OrdemPagto`;
  }

  buscarTransacoesSemOrdemPagto(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-transacoes-sem-ordem-pagto`, filtro);
  }
}
