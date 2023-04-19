import { ModeloBase } from './../models/modelo-balse';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ClienteSenhaModel } from '../models/cliente-senha.model';

@Injectable()
export class UsuarioClienteService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/UsuarioCliente`;
  }

  buscarTodosAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-todos-ativos`);
  }

  alterarSenha(cliente: ClienteSenhaModel) {
    return this.http.put<ModeloBase>(`${this.caminhoApi}/alterar-senha`, cliente);
  }
}
