import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class SessaoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Sessao`;
  }

  buscarTodos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-todos`);
  }
}
