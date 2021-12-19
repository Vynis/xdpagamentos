import { GestaoExtratoService } from './../../../@core/services/gestao-extrato-service';
import { ContaCaixaModel } from './../../../@core/models/contacaixa.model';
import { ContaCaixaService } from './../../../@core/services/conta-caixa.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { LocalDataSource, Ng2SmartTableComponent } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { ResumoLancamentosModel } from '../../../@core/models/resumo-lancamentos.model';
import { RelContaEstabelecimentoModel } from '../../../@core/models/rel-conta-estabelecimento.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';

@Component({
  selector: 'ngx-gestao-extrato-lista',
  templateUrl: './gestao-extrato-lista.component.html',
  styleUrls: ['./gestao-extrato-lista.component.scss']
})
export class GestaoExtratoListaComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  min: Date;
  max: Date;
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  resumoLancamentos: ResumoLancamentosModel = new ResumoLancamentosModel();
  @ViewChild('table') smartTable: Ng2SmartTableComponent;
  listaContaCaixa: ContaCaixaModel[] = [];
  lisRelContaEstabelecimento: RelContaEstabelecimentoModel[] = [];

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    dtHrLancamentoFormatada: {
      title: 'Dt. Lançamento ',
      type: 'string',
    },
    estabelecimentoFormatada: {
      title: 'Estabelecimento',
      type: 'string',
    },
    descricao: {
      title: 'Descrição',
      type: 'string',
    },
    vlBruto: {
      title: 'Vl. Bruto',
      type: 'string',
    },
    tipo: {
      title: 'Tipo',
      type: 'string',
    },
    usuarioFormatada: {
      title: 'Usuario',
      type: 'string',
    },
    dtHrCreditoFormatada: {
      title: 'Dt. Hr. Operação',
      type: 'string',
    }
  }

  constructor(
    private contaCaixaSerivce: ContaCaixaService, 
    private gestaoExtratoService: GestaoExtratoService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.buscarContaCaixa();
    this.createFormFiltro();

    this.settings.columns = this.columns;
    this.settings.actions.custom = [];
    this.settings.actions.custom.push({ name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'});
    this.settings.rowClassFunction = (row) => { 
      if (row.data.codRef === 'LANC-EXTRATO-CRED-DEB')
        return ''
      else
        return 'remove'
    };
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      contaCaixa: [0 ,Validators.required],
      estabelecimento: [0 ,Validators.required],
      descricao: [''],
      nomeCliente: [''],
      dtInicial: [new Date(), Validators.required],
      dtFinal: [new Date(), Validators.required],
      valorliquido: [''],
      tipo: ['T']
    })
  }


  buscarContaCaixa() {
    this.contaCaixaSerivce.buscarAtivos().subscribe(
      res => {
        if (!res.success)
          return;

        this.listaContaCaixa = res.data;
      }
    )
  }

  buscaEstabelecimento(id: number) {
    this.contaCaixaSerivce.buscarContaCaixaEstabelecimentoPorId(id).subscribe(
      res => {
        if (!res.success)
          return;
          
        this.lisRelContaEstabelecimento = res.data;
      }
    )
  }

  itemSelecionado(evento) {
    this.formularioFiltro.controls.estabelecimento.setValue(0);
    this.buscaEstabelecimento(evento);
  }

  pesquisar() {
    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    this.existeErro = false;
    this.realizouFiltro = false;

    //Validacoes
    if (controls.contaCaixa.value == 0) {
      this.existeErro = true;
      return;
    }

    if ((controls.dtInicial.value === '' || !this.ehData(controls.dtInicial.value)) || (controls.dtFinal.value === '' || !this.ehData(controls.dtInicial.value))) {
      this.existeErro = true;
      return;
    }

    var item  = new FiltroItemModel();
    item.property = 'RelContaEstabelecimento.CocId';
    item.filterType = FilterTypeConstants.EQUALS;
    item.value = controls.contaCaixa.value;
    listaItem.push(item);


    if (controls.estabelecimento.value !== 0) {
      var item  = new FiltroItemModel();
      item.property = 'RceId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.estabelecimento.value;
      listaItem.push(item);
    }

    var item  = new FiltroItemModel();
    item.property = 'DtHrLancamento';
    item.filterType = FilterTypeConstants.GREATERTHANEQUALS;
    item.value = new Date(controls.dtInicial.value).toLocaleDateString();
    listaItem.push(item);


    var item  = new FiltroItemModel();
    item.property = 'DtHrLancamento';
    item.filterType = FilterTypeConstants.LESSTHANEQUALS;
    item.value =new Date(controls.dtFinal.value).toLocaleDateString() + ' 23:59';
    listaItem.push(item);


    if (controls.descricao.value !== ''){
      var item  = new FiltroItemModel();
      item.property = 'Descricao';
      item.filterType = FilterTypeConstants.CONTAINS;
      item.value = controls.descricao.value;
      listaItem.push(item);
    }

    if (controls.nomeCliente.value !== ''){ 
      var item  = new FiltroItemModel();
      item.property = 'Cliente.Nome';
      item.filterType = FilterTypeConstants.CONTAINS;
      item.value = controls.valorliquido.value;
      listaItem.push(item);
    }


    filtro.filtro = listaItem;
    this.buscarDados(filtro);
  }

  buscarDados(filtro: PaginationFilterModel) {
    this.gestaoExtratoService.buscar(filtro).subscribe(
      res => {
        if (res.success) {
          this.realizouFiltro = true;
          this.source.load(res.data.listaGestaoPagamentos);

          this.settings.actions.custom = [];
          const controls = this.formularioFiltro.controls;
          this.resumoLancamentos.contaCaixa = this.listaContaCaixa.filter(x => x.id == controls.contaCaixa.value)[0].descricao;
          this.resumoLancamentos.estabelecimento = controls.estabelecimento.value === 0 ? '---Todos---' : this.lisRelContaEstabelecimento.filter(x => x.id == controls.estabelecimento.value)[0].estabelecimento.nome;
          this.resumoLancamentos.periodo = `De ${ new Date(controls.dtInicial.value).toLocaleDateString() } até ${ new Date(controls.dtFinal.value).toLocaleDateString() }`;
          this.resumoLancamentos.saldoAnterior = res.data.saldoAnterior;
          this.resumoLancamentos.entradas = res.data.entradas;
          this.resumoLancamentos.saidas = res.data.saidas;
          this.resumoLancamentos.saldoAtual = res.data.saldoAtual;
        }
      }
    )
  }

  ehData(valor) {
    return (valor instanceof Date)
  }


  onCustom(event) {}

}
