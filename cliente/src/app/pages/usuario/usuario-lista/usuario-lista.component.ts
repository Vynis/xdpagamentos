import { Router } from '@angular/router';
import { UsuarioService } from './../../../@core/services/usuario.service';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { NbDialogService } from '@nebular/theme';
import { AlterarSenhaComponent } from '../alterar-senha/alterar-senha.component';

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
  carregaPagina: boolean = false;
  validaNovo: boolean = false;
  
  constructor(
    private usuarioService: UsuarioService, 
    private route: Router,
    private authService: AuthServiceService,
    ) { 
    this.validaPermissao();
  }

  ngOnInit(): void {
  }

  private validaPermissao() {
    this.authService.validaPermissaoTela(SessoesEnum.LISTA_USUARIOS);
    this.authService.permissaoUsuario().subscribe(res => {
      if (!res.success)
        return;

      const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_USUARIOS);
      const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_USUARIOS);
      this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_USUARIOS);
      this.carregaPagina = true;

      this.buscaDados();
      this.configuracaoesGrid(validaExclusao,validaAlteracao);

    });
  }

  configuracaoesGrid(validaExclusao: boolean = false, validaAlteracao: boolean = false){
    this.settings.columns = this.columns;
    this.settings.actions.custom = [];

    if (validaAlteracao)
    this.settings.actions.custom.push({ name: AcoesPadrao.EDITAR, title: '<i title="Editar" class="nb-edit"></i>'});

    if (validaExclusao)
      this.settings.actions.custom.push({ name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'});
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
