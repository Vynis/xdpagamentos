import { SessaoModel } from './../../../@core/models/sessao.model';
import { SessaoService } from './../../../@core/services/sessao.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NbMenuService } from '@nebular/theme';

@Component({
  selector: 'ngx-permissao',
  templateUrl: './permissao.component.html',
  styleUrls: ['./permissao.component.scss']
})
export class PermissaoComponent implements OnInit {

  sessao: SessaoModel;
  paramRef: string = '';

  constructor(private menuService: NbMenuService, 
    private sessaoService: SessaoService, 
    private activatedRoute: ActivatedRoute) {
  }

  goToHome() {
    this.menuService.navigateHome();
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      this.paramRef = params.ref;
      this.buscarSessoes();
    });
  }

  buscarSessoes() {
    this.sessaoService.buscarTodos().subscribe(
      res => {
        if (res.success) {
          const sessoes = <SessaoModel[]>res.data;

          const filter = sessoes.filter(x => x.referencia == this.paramRef);

          if (filter.length > 0) {
            this.sessao = filter[0];
          }
        }
      }
    )
  }

}
