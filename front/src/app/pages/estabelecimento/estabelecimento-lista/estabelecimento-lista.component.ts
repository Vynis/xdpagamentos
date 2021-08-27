import { EstabelecimentoService } from './../../../@core/services/estabelecimento.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Router } from '@angular/router';
import { ToastService } from '../../../@core/services/toast.service';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';

@Component({
  selector: 'ngx-estabelecimento-lista',
  templateUrl: './estabelecimento-lista.component.html',
  styleUrls: ['./estabelecimento-lista.component.scss']
})
export class EstabelecimentoListaComponent implements OnInit {

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    nome: {
      title: 'Nome',
      type: 'string',
    },
    cnpjCpf: {
      title: 'Documento',
      type: 'string',
    },
    numEstabelecimento: {
      title: 'N. Estabelecimento',
      type: 'string',
    },
    cidade: {
      title: 'Cidade',
      type: 'string',
    },
    estado: {
      title: 'UF',
      type: 'string',
    },
    status: {
      title: 'Status',
      type: 'string',
    }
  }


  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  formulario: FormGroup;

  constructor(
    private estabelecimentoService: EstabelecimentoService,
    private toastService : ToastService,
    private route: Router,
    private fb: FormBuilder
  ) { 
    this.configuracaoesGrid();
    this.buscaDados(new PaginationFilterModel);
  }

  ngOnInit(): void {
    this.createForm();
  }

  configuracaoesGrid(){
    this.settings.columns = this.columns;
    this.settings.actions.custom = [
      { name: AcoesPadrao.EDITAR, title: '<i title="Editar" class="nb-edit"></i>'},
      { name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'}
    ];
  }

  createForm() {
    this.formulario = this.fb.group({
      tipo: ['0'],
      descricao: [''],
      status: [null]
    })
  }

  buscaDados(filtro: PaginationFilterModel) {
    this.estabelecimentoService.buscar(filtro).subscribe(
      res => {
        if (res.success) {
          this.source.load(res.data);
        }
      },
      err => {
        alert(err)
      }
    )
  }

  onCustom(event) {
    switch (event.action) {
      case AcoesPadrao.EDITAR:
        this.route.navigateByUrl(`/pages/estabelecimento/cadastro/edit/${event.data.id}`);
        break;
      default:
        break;
    }
  }

  pesquisar() {
    const controls = this.formulario.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    
    if (controls.tipo.value !== '0') {
      var item  = new FiltroItemModel();
      item.property = controls.tipo.value == 1 ? 'Nome' : 'CnpjCpf' ;
      item.filterType = controls.tipo.value == 1 ? FilterTypeConstants.CONTAINS : FilterTypeConstants.EQUALS;
      item.value = controls.descricao.value;
      listaItem.push(item);
    }

    if (controls.status.value !== null) {
      var item  = new FiltroItemModel();
      item.property = 'Status';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.status.value;
      listaItem.push(item);
    }

    filtro.filtro = listaItem;
    this.buscaDados(filtro);

  }

}
