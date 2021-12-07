import { ClienteModel } from './../../../@core/models/cliente.model';
import { ClienteService } from './../../../@core/services/cliente.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { LocalDataSource } from 'ng2-smart-table';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { Router } from '@angular/router';
import { TheadTitlesRowComponent } from 'ng2-smart-table/lib/components/thead/rows/thead-titles-row.component';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { GestaoPagamentoService } from '../../../@core/services/gestao-pagamento-service';
import { ResumoLancamentosModel } from '../../../@core/models/resumo-lancamentos.model';

@Component({
  templateUrl: './gestao-pagamento-lista.component.html',
  styleUrls: ['./gestao-pagamento-lista.component.scss']
})
export class GestaoPagamentoListaComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  min: Date;
  max: Date;
  listaClientes: ClienteModel[];
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  resumoLancamentos: ResumoLancamentosModel = new ResumoLancamentosModel();

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    dtHrLancamentoFormatada: {
      title: 'Dt. Lançamento ',
      type: 'string',
    },
    descricao: {
      title: 'Descrição',
      type: 'string',
    },
    vlLiquido: {
      title: 'Vl. Liquido',
      type: 'string',
    },
    tipo: {
      title: 'Tipo',
      type: 'string',
    },
    usuario: {
      title: 'Usuario',
      type: 'string',
    },
    dtHrCreditoFormatada: {
      title: 'Dt. Hr. Operação',
      type: 'string',
    }
  }

  constructor(
    private clienteService: ClienteService, 
    private gestaoPagtoService: GestaoPagamentoService,
    private route: Router,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.buscaClientes();
    this.createFormFiltro();

    this.settings.columns = this.columns;
    this.settings.actions.custom = [];
    this.settings.actions.custom.push({ name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'});
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      cliente: [0, Validators.required],
      descricao: [''],
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      valorliquido: [''],
      tipo: ['T']
    })
  }


  pesquisar(): void {

    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    

    if (controls.cliente.value !== 0) {
      var item  = new FiltroItemModel();
      item.property = 'CliId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.cliente.value;
      listaItem.push(item);
    }

    if ((controls.dtInicial.value !== '' && this.ehData(controls.dtInicial.value))  && (controls.dtFinal.value !== '' && this.ehData(controls.dtInicial.value)) ) {
      var item  = new FiltroItemModel();
      item.property = 'DtHrLancamento';
      item.filterType = FilterTypeConstants.GREATERTHANEQUALS;
      item.value = new Date(controls.dtInicial.value).toLocaleDateString();
      listaItem.push(item);


      var item  = new FiltroItemModel();
      item.property = 'DtHrLancamento';
      item.filterType = FilterTypeConstants.LESSTHANEQUALS;
      item.value =new Date(controls.dtFinal.value).toLocaleDateString();
      listaItem.push(item);
  
    }


    if (controls.descricao.value !== ''){
      var item  = new FiltroItemModel();
      item.property = 'Descricao';
      item.filterType = FilterTypeConstants.CONTAINS;
      item.value = controls.descricao.value;
      listaItem.push(item);
    }

    if (controls.valorliquido.value !== ''){ 
      var item  = new FiltroItemModel();
      item.property = 'VlLiquido';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.valorliquido.value;
      listaItem.push(item);
    }

    filtro.filtro = listaItem;
    this.buscarDados(filtro);

  }

  buscarDados(filtro: PaginationFilterModel) {
    this.gestaoPagtoService.buscar(filtro).subscribe(
      res => {
        if (res.success) {
          this.realizouFiltro = true;
          this.source.load(res.data.listaGestaoPagamentos);

          const controls = this.formularioFiltro.controls;
          this.resumoLancamentos.cliente = this.listaClientes.filter(x => x.id == controls.cliente.value)[0].nome;
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

  buscaClientes() {
    this.clienteService.buscarAtivos().subscribe(
      res => {
        if (!res.success)
          return;

        this.listaClientes = res.data;
      }
    )
  }

  onCustom(event) {
    switch (event.action) {
      case AcoesPadrao.EDITAR:
        this.route.navigateByUrl(`/pages/conta/cadastro/edit/${event.data.id}`);
        break;
      default:
        break;
    }
  }

}
