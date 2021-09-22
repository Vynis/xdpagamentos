import { Router } from '@angular/router';
import { UsuarioService } from './../../../@core/services/usuario.service';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';

@Component({
  selector: 'ngx-usuario-lista',
  templateUrl: './usuario-lista.component.html',
  styleUrls: ['./usuario-lista.component.scss']
})
export class UsuarioListaComponent implements OnInit {

  
  @ViewChild('filtroNome', { static: true }) filtroNome: ElementRef;

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    nome: {
      title: 'Nome',
      type: 'string',
    },
    cpf: {
      title: 'CPF',
      type: 'string',
    },
    status: {
      title: 'Status',
      type: 'string',
    }
  }

  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  
  constructor(private usuarioService: UsuarioService, private route: Router) { 
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

  onCustom(event) {
    switch (event.action) {
      case AcoesPadrao.EDITAR:
        this.route.navigateByUrl(`/pages/usuario/cadastro/edit/${event.data.id}`);
        break;
      default:
        break;
    }
  }

  buscaDados(nome = '') {
    this.usuarioService.buscar(nome).subscribe(
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

  pesquisar() {
    this.buscaDados(this.filtroNome.nativeElement.value);
  }
  

}
