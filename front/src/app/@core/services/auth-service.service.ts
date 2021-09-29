import { SessaoModel } from './../models/sessao.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ModeloBase } from '../models/modelo-balse';
import { Router } from '@angular/router';
import { PermissaoModel } from '../models/permissao.model';


@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  caminhoApi: string = '';

  constructor(private http: HttpClient, private router: Router) { 
    this.caminhoApi =  `${environment.api}/Autenticacao`;
  }

  permissaoUsuario() {
    return this.http.get<ModeloBase>(`${this.caminhoApi}/permissao-usuario`);
  }

  validaPermissaoTela(referencia: string) {
    this.permissaoUsuario().subscribe(res => {

      if (!res.success) {
        this.router.navigateByUrl('/pages/miscellaneous/404');
        return;
      }

      let sessao: SessaoModel[] = [];
      const dados = <PermissaoModel[]>res.data;

      dados.forEach(res => {
        sessao.push(res.sessao);
      });
      
      const filtro = sessao.filter(x => x.referencia === referencia);

      if (filtro.length === 0)
        this.router.navigateByUrl(`/pages/miscellaneous/403/${referencia}`);

    });

  }

  validaPermissaoAvulsa(listaPermissao: PermissaoModel[], referencia: string)  { 
      let sessao: SessaoModel[] = [];

      listaPermissao.forEach(res => {
        sessao.push(res.sessao);
      });
    
      const filtro = sessao.filter(x => x.referencia === referencia);

      if (filtro.length === 0)
        return false;

      return true;
  }

}
