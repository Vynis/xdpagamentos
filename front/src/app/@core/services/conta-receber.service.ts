import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ModeloBase } from "../models/modelo-balse";
import { PaginationFilterModel } from "../models/configuracao/paginationfilter.model";
import { ContaReceberModel } from "../models/conta-receber.model";

@Injectable()
export class ContaReceberService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/ContaReceber`;
  }

  buscar(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-com-filtro`, filtro);
  }
  
  buscarTodos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-todos`);
  }

  inserir(model : ContaReceberModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : ContaReceberModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  remover(id : number) {
    return this.http.delete<ModeloBase>(`${this.caminhoApi}/deletar/${id}`);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }
}