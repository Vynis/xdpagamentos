import { ModeloBase } from './../models/modelo-balse';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable()
export class UsuarioService {

  caminhoApi: string = '';

  constructor(private http: HttpClient) { 
    this.caminhoApi =  `${environment.api}/Usuario`;
  }

  buscar(nome: string = '') {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/buscar-usuarios-filtro?nome=${nome}`);
  }
}
