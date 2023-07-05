import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CentroCustoModel } from '../../../@core/models/centro-custo.model';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { ContaReceberService } from '../../../@core/services/conta-receber.service';
import { Router } from '@angular/router';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { CentroCustoService } from '../../../@core/services/centro-custo.service';
import { SweetalertService } from '../../../@core/services/sweetalert.service';
import { ToastService } from '../../../@core/services/toast.service';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { FluxoCaixaService } from '../../../@core/services/fluxo-caixa.service';
import { SweetAlertIcons } from '../../../@core/enums/sweet-alert-icons-enum';

@Component({
  selector: 'ngx-conta-receber-lista',
  templateUrl: './conta-receber-lista.component.html',
  styleUrls: ['./conta-receber-lista.component.scss']
})
export class ContaReceberListaComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  min: Date;
  max: Date;
  listaCentroCusto: CentroCustoModel[];
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    descricao: {
      title: 'Descrição',
      type: 'string',
    },
    descricaoCentroCusto: {
      title: 'Centro Custo',
      type: 'string',
    },
    dtEmissaoFormatada: {
      title: 'Dt. Emissão',
      type: 'string',
    },
    dtVencimentoFormatada: {
      title: 'Dt. Vencimento',
      type: 'string',
    }, 
    fluxoCaixa: {
      title: 'Fluxo Caixa',
      type: 'string',
    },   
    status: {
      title: 'Status',
      type: 'string',
    }
  }

  constructor(
    private contaReceberService: ContaReceberService, 
    private centroCustoService: CentroCustoService,
    private toastService : ToastService,
    private sweetAlertService: SweetalertService,
    private route: Router,
    private fb: FormBuilder,
    private authService: AuthServiceService,
    private fluxoCaixaService: FluxoCaixaService
  ) { }


  ngOnInit() {
    this.buscaCentroCusto();
    this.createFormFiltro();
    this.pesquisar();

    this.settings.columns = this.columns;
    this.settings.actions.custom = [];
    this.settings.actions.custom.push({ name: AcoesPadrao.EDITAR, title: '<i title="Editar" class="nb-edit"></i>'});
    this.settings.actions.custom.push({ name: 'Baixar', title: '<i title="Aprovar" class="nb-checkmark"></i>'});
    this.settings.actions.custom.push({ name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'});
    this.settings.actions.custom.push({ name: 'Restaurar', title: '<i title="Restaurar" class="nb-shuffle"></i>'});
    this.settings.rowClassFunction = (row) => { 
      if (row.data.status === 'NP'){
        return 'restaurar'
      }
      else {
        return 'remove baixa edit'
      }
        
    };
   
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      centroCusto: ['0'],
      status: ['0'],
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      descricao: [''],
      tipoPeriodo: ['1']
    })
  }

  ehData(valor) {
    return (valor instanceof Date)
  }

  pesquisar() {
    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    this.existeErro = false;

    if ((controls.dtInicial.value === '' || !this.ehData(controls.dtInicial.value)) || (controls.dtFinal.value === '' || !this.ehData(controls.dtInicial.value))) {
      this.existeErro = true;
      return;
    }

    var item  = new FiltroItemModel();
    item.property = 'Descricao';
    item.filterType = FilterTypeConstants.CONTAINS;
    item.value = controls.descricao.value ;
    listaItem.push(item);

    var item  = new FiltroItemModel();
    item.property = controls.tipoPeriodo.value == '1' ? 'DataVencimento' : 'DataEmissao';
    item.filterType = FilterTypeConstants.GREATERTHANEQUALS;
    item.value = new Date(controls.dtInicial.value).toLocaleDateString();
    listaItem.push(item);


    var item  = new FiltroItemModel();
    item.property = controls.tipoPeriodo.value == '1' ? 'DataVencimento' : 'DataEmissao';
    item.filterType = FilterTypeConstants.LESSTHANEQUALS;
    item.value =new Date(controls.dtFinal.value).toLocaleDateString() + ' 23:59';
    listaItem.push(item);

    if (controls.centroCusto.value != '0') {
      var item  = new FiltroItemModel();
      item.property = 'CecId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.centroCusto.value;
      listaItem.push(item);
    }

    if (controls.status.value != '0') {
      var item  = new FiltroItemModel();
      item.property = 'Status';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.status.value;
      listaItem.push(item);
    }

    filtro.filtro = listaItem;
    this.buscarDados(filtro);
  }

  buscarDados(filtro: PaginationFilterModel) {
    this.contaReceberService.buscar(filtro).subscribe(
      res => {
        if (res.success) {
          //this.realizouFiltro = true;
          this.source.load(res.data);
        }
      }
    )
  }

  onCustom(event) {
    switch (event.action) {
      case 'Baixar':
        this.route.navigateByUrl(`/pages/conta-receber/baixa/${event.data.id}`);
        break;
      case AcoesPadrao.REMOVER:
        this.sweetAlertService.msgPadrao().then(
          res => {
            if (res.isConfirmed){
              this.excluir(event.data.id);
            } 
          }
        )
        break;
      case AcoesPadrao.EDITAR:
        this.route.navigateByUrl(`/pages/conta-receber/cadastro/edit/${event.data.id}`);
        break;
      case 'Restaurar':
          this.sweetAlertService.msgPadrao('Tem certeza que deseja restaurar este lançamento?', ' ', SweetAlertIcons.QUESTION).then(
            res => {
              if (res.isConfirmed) {
                this.fluxoCaixaService.restaurar(event.data.id,'CR').subscribe(
                  res => {
                    if (res.success) 
                     this.sweetAlertService.msgAvulsa('Conta restaurada com sucesso!', SweetAlertIcons.SUCESS, '');
                     this.pesquisar();
                  }
                )
              }
            }
          )
          break;
      default:
        break;
    }
  }

  excluir(id) {}

  buscaCentroCusto() {
    this.centroCustoService.buscarAtivos().subscribe(
      res => {
        if (!res.success)
          return;

        this.listaCentroCusto = res.data;
      }
    )
  }

}
