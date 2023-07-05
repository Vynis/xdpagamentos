import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ModeloBase } from "../models/modelo-balse";
import { CentroCustoModel } from "../models/centro-custo.model";

@Injectable()
export class CentroCustoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/CentroCusto`;
  }

  buscarTodos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-todos`);
  }

  inserir(model : CentroCustoModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : CentroCustoModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  remover(id : number) {
    return this.http.delete<ModeloBase>(`${this.caminhoApi}/deletar/${id}`);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }
}