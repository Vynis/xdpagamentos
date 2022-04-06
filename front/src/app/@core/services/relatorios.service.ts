import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { PaginationFilterModel } from "../models/configuracao/paginationfilter.model";
import { ModeloBase } from "../models/modelo-balse";
import { ParamOrdemPagtoModel } from "../models/param-ordem-pagto.model";

@Injectable()
export class RelatoriosService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Relatorios`;
  }

  buscaRelatorioSolicitacoes(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-relatorio-solicitacao`, filtro);
  }

  buscaRelatorioSaldoClientes(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-relatorio-saldo-clientes`, filtro);
  }

  buscaRelatorioSaldoContaCorrente(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-relatorio-saldo-conta-corrente`, filtro);
  }

}