import { PaginationFilterModel } from './../../../@core/models/configuracao/paginationfilter.model';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalDataSource } from 'ng2-smart-table';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { ClienteService } from '../../../@core/services/cliente.service';

@Component({
  selector: 'ngx-cliente-lista',
  templateUrl: './cliente-lista.component.html',
  styleUrls: ['./cliente-lista.component.scss']
})
export class ClienteListaComponent implements OnInit {

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
    cidade: {
      title: 'Cidade',
      type: 'string',
    },
    estado: {
      title: 'Estado',
      type: 'string',
    },
    status: {
      title: 'Status',
      type: 'string',
    }
  }

  
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();

  constructor(private clienteService: ClienteService, private route: Router) {
    this.configuracaoesGrid();
    this.buscaDados(new PaginationFilterModel);
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

  buscaDados(filtro: PaginationFilterModel) {
    this.clienteService.buscar(filtro).subscribe(
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
        this.route.navigateByUrl(`/pages/cliente/cadastro/edit/${event.data.id}`);
        break;
      default:
        break;
    }
  }

}
