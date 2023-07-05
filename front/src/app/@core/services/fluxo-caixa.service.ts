import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ModeloBase } from "../models/modelo-balse";
import { FluxoCaixaModel } from "../models/fluxo-caixa.model";

@Injectable()
export class FluxoCaixaService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/FluxoCaixa`;
  }

  buscarContas(conta: string, id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-contas/${conta}/${id}`);
  }

  inserir(model : FluxoCaixaModel, tipo: string) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir/${tipo}`, model);
  }

  restaurar(idConta: number, tipo: string) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/restaurar/${tipo}/${idConta}`);
  }
}