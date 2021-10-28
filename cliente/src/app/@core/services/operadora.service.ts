import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class OperadoraService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Operadora`;
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }
}
