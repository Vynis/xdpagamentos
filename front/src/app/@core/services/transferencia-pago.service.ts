import { ParamOrdemPagtoModel } from '../models/param-ordem-pagto.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { PaginationFilterModel } from '../models/configuracao/paginationfilter.model';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class TransferenciaPagtoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/TransferenciaTransacao`;
  }

  buscarTransacoesSemOrdemPagto(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-transacoes-sem-ordem-pagto`, filtro);
  }

  gerarOrdemPagto(parametro: ParamOrdemPagtoModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/gerar-ordem-pagto`, parametro);
  }
}
