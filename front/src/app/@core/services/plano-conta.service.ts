import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ModeloBase } from "../models/modelo-balse";
import { CentroCustoModel } from "../models/centro-custo.model";

@Injectable()
export class PlanoContaService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/PlanoConta`;
  }

  buscarAtivos(tipo: string) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos/${tipo}`);
  }
}