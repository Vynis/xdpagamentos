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
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbDateService } from '@nebular/theme';
import { FiltroItemModel } from '../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../@core/enums/filter-type.enum';

@Component({
  selector: 'ngx-geracao-ordem-pagto',
  templateUrl: './geracao-ordem-pagto.component.html',
  styleUrls: ['./geracao-ordem-pagto.component.scss']
})
export class GeracaoOrdemPagtoComponent implements OnInit {

  listaEstabelecimento: EstabelecimentoModel[] = [];
  listaRelContaCaixaEstabeleicmento: RelContaEstabelecimentoModel[] = [];
  listaTransacoesSemOrdemPagto: TransacoesSemOrdemPagtoPorClienteModel[] = [];
  listaTransacoesSemOrdemPagtoSelecionado: TransacoesSemOrdemPagtoPorClienteModel[] = [];
  formularioFiltro: FormGroup;
  formularioGeracao: FormGroup;
  min: Date;
  max: Date;
  realizouFiltro: boolean = false;


  constructor(
    private estabelecimentoService: EstabelecimentoService,
    private contaCaixaService: ContaCaixaService, 
    private orderPagtoService: OrdemPagtoService,
    private fb: FormBuilder,
    protected dateService: NbDateService<Date>
    ) { 
      // this.min = this.dateService.addMonth(this.dateService.today(), -1);
      // this.max = this.dateService.addMonth(this.dateService.today(), 1);
    }

  ngOnInit(): void {
    this.buscarEstabelecimentos();
    this.buscarContaCaixaRelEstabelecimento();

    this.createFormFiltro();

  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      periodoGeral: [false],
      estabelecimento: [null, Validators.required],
      dtInicial: [new Date(), Validators.required ],
      dtFinal: [new Date(), Validators.required],
      status: ['NP', Validators.required]
    })
  }

  createFormGeracao() {

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
          this.realizouFiltro = true;
        }
      },
      err => {
        alert(err)
      }
    )
  }


  tranformaNomeEstabelecimento(texto) {
    if (texto.length > 15)
      texto = texto.substring(0,12) + '...';

    return texto;
  }

  selecionaCliente(evento: boolean, item : TransacoesSemOrdemPagtoPorClienteModel) {
    if (evento) {
      this.listaTransacoesSemOrdemPagtoSelecionado.push(item);
    }
    else {
      this.listaTransacoesSemOrdemPagtoSelecionado = this.listaTransacoesSemOrdemPagtoSelecionado.filter(c => c !== item);
    }
  }

  pesquisar() {
    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    

    if (controls.estabelecimento.value !== null) {
      var item  = new FiltroItemModel();
      item.property = 'EstId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.estabelecimento.value;
      listaItem.push(item);
    }


    if ((controls.dtInicial.value !== '' && this.ehData(controls.dtInicial.value))  && (controls.dtFinal.value !== '' && this.ehData(controls.dtInicial.value)) ) {
      var item  = new FiltroItemModel();
      item.property = 'DataOperacao';
      item.filterType = FilterTypeConstants.GREATERTHANEQUALS;
      item.value = new Date(controls.dtInicial.value).toLocaleDateString();
      listaItem.push(item);


      var item  = new FiltroItemModel();
      item.property = 'DataOperacao';
      item.filterType = FilterTypeConstants.LESSTHANEQUALS;
      item.value =new Date(controls.dtFinal.value).toLocaleDateString();
      listaItem.push(item);
  
    }

    // if (controls.status.value !== null) {
    //   var item  = new FiltroItemModel();
    //   item.property = 'Status';
    //   item.filterType = FilterTypeConstants.EQUALS;
    //   item.value = controls.status.value;
    //   listaItem.push(item);
    // }



    filtro.filtro = listaItem;
    this.buscaDados(filtro);

  }

  ehData(valor) {
    return (valor instanceof Date)
  }




}
