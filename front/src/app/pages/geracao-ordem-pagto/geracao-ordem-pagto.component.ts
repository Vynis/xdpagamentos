import { ContaCaixaService } from './../../@core/services/conta-caixa.service';
import { EstabelecimentoService } from './../../@core/services/estabelecimento.service';
import { Component, OnInit } from '@angular/core';
import { OrdemPagtoService } from '../../@core/services/ordem-pagto.service';
import { EstabelecimentoModel } from '../../@core/models/estabelecimento.model';
import { RelContaEstabelecimentoModel } from '../../@core/models/rel-conta-estabelecimento.model';
import { PaginationFilterModel } from '../../@core/models/configuracao/paginationfilter.model';
import { TransacoesSemOrdemPagtoPorClienteModel } from '../../@core/models/transacoes-sem-ordem-pagto-cliente.model';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { VwTransacoesSemOrdemPagtoModel } from '../../@core/models/vw-trasacoes-sem-ordem-pagto.model';
import { CurrencyFormatPipe } from '../../@core/utils/currency-format-pipe';

@Component({
  selector: 'ngx-geracao-ordem-pagto',
  templateUrl: './geracao-ordem-pagto.component.html',
  styleUrls: ['./geracao-ordem-pagto.component.scss']
})
export class GeracaoOrdemPagtoComponent implements OnInit {

  listaEstabelecimento: EstabelecimentoModel[] = [];
  listaRelContaCaixaEstabeleicmento: RelContaEstabelecimentoModel[] = [];
  listaTransacoesSemOrdemPagto: TransacoesSemOrdemPagtoPorClienteModel[] = [];


  constructor(private estabelecimentoService: EstabelecimentoService,private contaCaixaService: ContaCaixaService, private orderPagtoService: OrdemPagtoService) { }

  ngOnInit(): void {
    this.buscarEstabelecimentos();
    this.buscarContaCaixaRelEstabelecimento();
    this.buscaDados(new PaginationFilterModel());

  }

  buscarEstabelecimentos() {
    this.estabelecimentoService.buscarAtivos().subscribe(
      res => { 
        if (res.success) {
          this.listaEstabelecimento = res.data;
        }
      }
    )
  }

  buscarContaCaixaRelEstabelecimento() {
    this.contaCaixaService.buscarContaCaixaEstabelecimento().subscribe(
      res => {
        if (res.success) {
          this.listaRelContaCaixaEstabeleicmento = res.data;
        }
      }
    )
  }

  buscaDados(filtro: PaginationFilterModel) {
    this.orderPagtoService.buscarTransacoesSemOrdemPagto(filtro).subscribe(
      res => {
        if (res.success) {
          this.listaTransacoesSemOrdemPagto = res.data;
        }
      },
      err => {
        alert(err)
      }
    )
  }

  tranformarData(data) {
    var datePipe = new DatePipe('en-US');
    return datePipe.transform(data, 'dd/MM/yyyy');
  }

  transformarMoeda(valor) {
    var valorPipe = new CurrencyFormatPipe();
    return valorPipe.transform( Number(valor.replace(',', '.')), 'BRL');
  }

  tranformaNomeEstabelecimento(texto) {
    if (texto.length > 15)
      texto = texto.substring(0,12) + '...';

    return texto;
  }

  somatorioVlBrutoTotal(transacao: VwTransacoesSemOrdemPagtoModel[]) {
    var valor = 0;
    var valorPipe = new CurrencyPipe('en-US');
    transacao.forEach(x => valor+= Number(x.vlBruto));
    return valorPipe.transform(Number(valor), 'BRL');;
  }

}
