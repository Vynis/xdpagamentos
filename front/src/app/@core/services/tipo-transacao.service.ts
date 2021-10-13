import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';

@Injectable()
export class TipoTransacaoService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/TipoTransacao`;
  }

  buscarPadrao() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-padrao`);
  }
}
