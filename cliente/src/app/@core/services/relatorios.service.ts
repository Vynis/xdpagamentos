import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ModeloBase } from "../models/modelo-balse";

@Injectable()
export class RelatoriosService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Relatorios`;
  }

  buscaGraficoVendas() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-grafico-vendas`);
  }

}