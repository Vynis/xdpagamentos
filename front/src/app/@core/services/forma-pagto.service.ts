import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class FormaPagtoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/FormaPagto`;
  }

  buscarAtivos(cliId: number = 0) {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos/${cliId}`);
  }
}
