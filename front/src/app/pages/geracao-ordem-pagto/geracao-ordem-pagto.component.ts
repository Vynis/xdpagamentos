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
import { ParamOrdemPagtoModel } from '../../@core/models/param-ordem-pagto.model';
import { ToastService } from '../../@core/services/toast.service';
import { ToastPadrao } from '../../@core/enums/toast.enum';

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
  existeErro:boolean = false;
  existeErro2:boolean = false;


  constructor(
    private estabelecimentoService: EstabelecimentoService,
    private contaCaixaService: ContaCaixaService, 
    private orderPagtoService: OrdemPagtoService,
    private fb: FormBuilder,
    protected dateService: NbDateService<Date>,
    private toastService : ToastService,
    ) { 
      // this.min = this.dateService.addMonth(this.dateService.today(), -1);
      // this.max = this.dateService.addMonth(this.dateService.today(), 1);
    }

  ngOnInit(): void {
    this.buscarEstabelecimentos();
    this.buscarContaCaixaRelEstabelecimento();

    this.createFormFiltro();
    this.createFormGeracao();

  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      periodoGeral: [false],
      estabelecimento: [0, Validators.required],
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      status: ['NP']
    })
  }

  createFormGeracao() {
    this.formularioGeracao = this.fb.group({
      conta: [null, Validators.required],
      dtLancamento: [new Date(), Validators.required]
    })
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

    // if (this.validacaoFiltro() === false)
    //   return;

    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    

    if (controls.estabelecimento.value !== 0) {
      var item  = new FiltroItemModel();
      item.property = 'EstId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.estabelecimento.value;
      listaItem.push(item);
    }

    if (!controls.periodoGeral.value) {

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

  validacaoFiltro() : boolean {
    const controls = this.formularioFiltro.controls;
    this.existeErro = false;

    if (this.formularioFiltro.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro = true;
      return false;
    }

    return true;
  }

  validacaoFormGeracao() : boolean {
    const controls = this.formularioGeracao.controls;
    this.existeErro2 = false;

    if (this.formularioGeracao.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro2 = true;
      return false;
    }

    if (this.listaTransacoesSemOrdemPagtoSelecionado.length == 0)
      return false;

    return true;
  }

  submit() {

    if (this.validacaoFormGeracao() === false)
       return;

    const controls = this.formularioGeracao.controls;

    var parametro = new ParamOrdemPagtoModel();

    parametro.idConta = controls.conta.value;
    parametro.dataLancamentoCredito = controls.dtLancamento.value;
    parametro.clientesSelecionados = this.listaTransacoesSemOrdemPagtoSelecionado;

    this.orderPagtoService.gerarOrdemPagto(parametro).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar a geracao', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Ordem de pagamento gerado com sucesso!');
        this.pesquisar();
      }
    )

  }




}
