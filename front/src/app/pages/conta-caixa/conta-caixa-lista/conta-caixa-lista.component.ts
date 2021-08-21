import { ContaCaixaService } from './../../../@core/services/conta-caixa.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Router } from '@angular/router';
import { ToastService } from '../../../@core/services/toast.service';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';

@Component({
  selector: 'ngx-conta-caixa-lista',
  templateUrl: './conta-caixa-lista.component.html',
  styleUrls: ['./conta-caixa-lista.component.scss']
})
export class ContaCaixaListaComponent implements OnInit {

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    descricao: {
      title: 'Descrição',
      type: 'string',
    },
    status: {
      title: 'Status',
      type: 'string',
    }
  }

  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();

  constructor(
    private contaCaixaService: ContaCaixaService,
    private toastService : ToastService,
    private route: Router
  ) { 
    this.configuracaoesGrid();
    this.buscaDados();
  }

  ngOnInit(): void {
  }

  configuracaoesGrid(){
    this.settings.columns = this.columns;
    this.settings.actions.custom = [
      { name: AcoesPadrao.EDITAR, title: '<i title="Editar" class="nb-edit"></i>'},
      { name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'}
    ];
  }

  buscaDados() {
    this.contaCaixaService.buscarTodos().subscribe(
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
        this.route.navigateByUrl(`/pages/conta/cadastro/edit/${event.data.id}`);
        break;
      default:
        break;
    }
  }


}
