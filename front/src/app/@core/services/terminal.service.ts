import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { PaginationFilterModel } from '../models/configuracao/paginationfilter.model';
import { ModeloBase } from '../models/modelo-balse';
import { TerminalModel } from '../models/terminal.model';

@Injectable()
export class TerminalService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Terminal`;
  }

  buscar(filtro: PaginationFilterModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-terminal-filtro`, filtro);
  }

  buscaPorId(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-id/${id}`);
  }

  inserir(model : TerminalModel) {
    return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
  }

  alterar(model : TerminalModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar`, model);
  }

  remover(id : number) {
    return this.http.delete<ModeloBase>(`${this.caminhoApi}/deletar/${id}`);
  }

  bucarTerminaisCliente(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buca-terminais-cliente/${id}`);
  }

}
