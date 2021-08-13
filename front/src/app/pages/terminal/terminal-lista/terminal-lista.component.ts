import { TerminalService } from './../../../@core/services/terminal.service';
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
  selector: 'ngx-terminal-lista',
  templateUrl: './terminal-lista.component.html',
  styleUrls: ['./terminal-lista.component.scss']
})
export class TerminalListaComponent implements OnInit {

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    numTerminal: {
      title: 'Numero do Terminal',
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
    private terminalService: TerminalService,
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

  createForm() {
    this.formulario = this.fb.group({
      numTerminal: [''],
      status: [null]
    })
  }

  configuracaoesGrid(){
    this.settings.columns = this.columns;
    this.settings.actions.custom = [
      { name: AcoesPadrao.EDITAR, title: '<i title="Editar" class="nb-edit"></i>'},
      { name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'}
    ];
  }

  buscaDados(filtro: PaginationFilterModel) {
    this.terminalService.buscar(filtro).subscribe(
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
        this.route.navigateByUrl(`/pages/terminal/cadastro/edit/${event.data.id}`);
        break;
      default:
        break;
    }
  }

  pesquisar() {
    const controls = this.formulario.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    
    if (controls.numTerminal.value !== '') {
      var item  = new FiltroItemModel();
      item.property = 'NumTerminal' ;
      item.filterType = FilterTypeConstants.EQUALS;
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
