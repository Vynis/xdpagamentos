import { ClienteSenhaModel } from './../models/cliente-senha.model';
import { PaginationFilterModel } from './../models/configuracao/paginationfilter.model';
import { ClienteModel } from './../models/cliente.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class ClienteService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Cliente`;
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }

  buscaDadosCliente(id: number) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-dados-cliente-logado/${id}`);
  }

  alterarSenha(cliente: ClienteSenhaModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar-senha`, cliente);
  }

  atualizarDadosBancariosCliente(model : ClienteModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/atualizar-dados-bancarios-cliente-logado`, model);
  }

}
