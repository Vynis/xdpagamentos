import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'ngx-conta-receber-itens-fluxo-caixa',
  templateUrl: './conta-receber-itens-fluxo-caixa.component.html',
  styleUrls: ['./conta-receber-itens-fluxo-caixa.component.scss']
})
export class ContaReceberItensFluxoCaixaComponent implements OnInit {
  @Input() idConta: any;

  constructor() { }

  ngOnInit() {
    console.log(this.idConta.value.listaFluxoCaixa.length);
  }
}
