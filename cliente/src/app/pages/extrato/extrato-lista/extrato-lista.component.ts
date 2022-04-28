import { GestaoPagamentoService } from './../../../@core/services/gestao-pagamento-service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { LocalDataSource, Ng2SmartTableComponent } from 'ng2-smart-table';
import { ClienteModel } from '../../../@core/models/cliente.model';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';

@Component({
  selector: 'ngx-extrato-lista',
  templateUrl: './extrato-lista.component.html',
  styleUrls: ['./extrato-lista.component.scss']
})
export class ExtratoListaComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  min: Date;
  max: Date;
  listaClientes: ClienteModel[];
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  @ViewChild('table') smartTable: Ng2SmartTableComponent;
  total: string = '0,00';

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
    tipoFormatado: {
      title: 'Tipo',
      type: 'string',
    },
    valorFormatado: {
      title: 'Valor',
      type: 'string',
    },
    vlBrutoTransacao: {
      title: 'Valor Bruto',
      type: 'string',
    },
    qtdParcelaTransacao: {
      title: 'Qtd. Parcela',
      type: 'string',
    },
    codAutorizacaoTransacao: {
      title: 'Cod. Autorização',
      type: 'string',
    },
    numCartaoTransacao: {
      title: 'Num. Cartão',
      type: 'string',
    },
    meioCapturaTransacao: {
      title: 'Meio Captura',
      type: 'string',
    },
    tipoOperacaoTransacao: {
      title: 'Tipo Operação',
      type: 'string',
    },
    statusFormatado: {
      title: 'Status',
      type: 'string'
    }
  }

  constructor(private fb: FormBuilder, private gestaoPagtoService: GestaoPagamentoService) { }

  ngOnInit(): void {
    this.createFormFiltro();

    
    this.settings.columns = this.columns;
    this.settings.actions.custom = [];

  }

  
  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      tipo: ['T']
    })
  }

  buscarDados(filtro: PaginationFilterModel) {
    this.gestaoPagtoService.buscar(filtro).subscribe(
      res => {
        if (res.success) {
          this.realizouFiltro = true;
          this.source.load(res.data.listaPagamentos);
          this.total = res.data.total;
        }
      }
    )
  }

  pesquisar(): void {
    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    this.existeErro = false;
    this.realizouFiltro = false;

    //Validacoes
    if ((controls.dtInicial.value === '' || !this.ehData(controls.dtInicial.value)) || (controls.dtFinal.value === '' || !this.ehData(controls.dtInicial.value))) {
      this.existeErro = true;
      return;
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

    if (controls.tipo.value !== 'T'){
      var item  = new FiltroItemModel();
      item.property = 'Tipo';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.tipo.value;
      listaItem.push(item);
     }


    filtro.filtro = listaItem;
    this.buscarDados(filtro);

  }

  ehData(valor) {
    return (valor instanceof Date)
  }

  onCustom(event) {}

}
