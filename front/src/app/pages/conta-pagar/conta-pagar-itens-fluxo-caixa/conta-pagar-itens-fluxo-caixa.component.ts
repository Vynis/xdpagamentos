import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'ngx-conta-pagar-itens-fluxo-caixa',
  templateUrl: './conta-pagar-itens-fluxo-caixa.component.html',
  styleUrls: ['./conta-pagar-itens-fluxo-caixa.component.scss']
})
export class ContaPagarItensFluxoCaixaComponent implements OnInit {
  @Input() idConta: any;

  constructor() { }

  ngOnInit() {
    console.log(this.idConta.value.listaFluxoCaixa.length);
  }

}
