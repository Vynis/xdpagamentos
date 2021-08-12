import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class BancoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Banco`;
  }

  buscarAtivos() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-por-ativos`);
  }
}
