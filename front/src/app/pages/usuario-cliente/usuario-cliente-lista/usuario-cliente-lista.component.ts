import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Router } from '@angular/router';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SweetalertService } from '../../../@core/services/sweetalert.service';
import { UsuarioClienteService } from '../../../@core/services/usuario-cliente.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { SweetAlertIcons } from '../../../@core/enums/sweet-alert-icons-enum';

@Component({
  selector: 'ngx-usuario-cliente-lista',
  templateUrl: './usuario-cliente-lista.component.html',
  styleUrls: ['./usuario-cliente-lista.component.scss']
})
export class UsuarioClienteListaComponent implements OnInit {

  formulario: FormGroup;

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    nome: {
      title: 'Nome',
      type: 'string',
    },
    email: {
      title: 'Email',
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

  constructor(private usuarioClienteService: UsuarioClienteService, 
    private route: Router,
    private authService: AuthServiceService,
    private fb: FormBuilder,
    private sweetAlertService: SweetalertService,) { 
      this.validaPermissao();
    }

    ngOnInit(): void {
      this.createForm();
    }
  
    createForm() {
      this.formulario = this.fb.group({
        nome: [''],
      })
    }
  
    private validaPermissao() {
      this.authService.validaPermissaoTela(SessoesEnum.LISTA_USUARIOS_CLIENTES);
      this.authService.permissaoUsuario().subscribe(res => {
        if (!res.success)
          return;
  
        const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_USUARIOS_CLIENTES);
        const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_USUARIOS_CLIENTES);
        this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_USUARIOS_CLIENTES);
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
          this.route.navigateByUrl(`/pages/usuario-cliente/cadastro/edit/${event.data.id}`);
          break;
        case AcoesPadrao.REMOVER:
            this.sweetAlertService.msgPadrao().then(
              res => {
                if (res.isConfirmed){
                  this.deletar(event.data.id);
                } 
              }
            )
            break;         
        default:
          break;
      }
    }
  
    buscaDados(nome = '') {
      this.usuarioClienteService.buscar(nome).subscribe(
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
      this.buscaDados(this.formulario.controls.nome.value);
  
    }
    
    deletar(id: number) {
      this.usuarioClienteService.remover(id).subscribe(
        res => {
          if (res.success) {
            this.sweetAlertService.msgAvulsa('Deletado', SweetAlertIcons.SUCESS ,'');
            this.pesquisar();
          } else {
  

            this.sweetAlertService.msgAvulsa('Erro',SweetAlertIcons.ERROR, `Não foi possivel excluir o registro porque tem vinculo`);
  
          }
        }
      )
    }

}
