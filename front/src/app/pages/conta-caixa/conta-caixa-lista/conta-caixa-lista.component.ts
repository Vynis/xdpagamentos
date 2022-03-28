import { ContaCaixaService } from './../../../@core/services/conta-caixa.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Router } from '@angular/router';
import { ToastService } from '../../../@core/services/toast.service';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { SweetalertService } from '../../../@core/services/sweetalert.service';
import { SweetAlertIcons } from '../../../@core/enums/sweet-alert-icons-enum';

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
  carregaPagina: boolean = false;
  validaNovo: boolean = false;

  constructor(
    private contaCaixaService: ContaCaixaService,
    private toastService : ToastService,
    private route: Router,
    private authService: AuthServiceService,
    private sweetAlertService: SweetalertService,
  ) { 
    this.validaPermissao();
  }

  ngOnInit(): void {
  }

  private validaPermissao() {
    this.authService.validaPermissaoTela(SessoesEnum.LISTA_CONTA_CAIXA);
    this.authService.permissaoUsuario().subscribe(res => {
      if (!res.success)
        return;

      const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_CONTA_CAIXA);
      const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_CONTA_CAIXA);
      this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_CONTA_CAIXA);
      this.carregaPagina = true;

      this.buscaDados();
      this.configuracaoesGrid(validaExclusao,validaAlteracao );

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


  deletar(id: number) {
    this.contaCaixaService.remover(id).subscribe(
      res => {
        if (res.success) {
          this.sweetAlertService.msgAvulsa('Deletado', SweetAlertIcons.SUCESS ,'');
          this.buscaDados();
        } else {

          let erros = '';
          res.data.forEach(e => {
            erros+= `,${e}`
          });

          this.sweetAlertService.msgAvulsa('Erro',SweetAlertIcons.ERROR, `Não foi possivel excluir o registro porque tem vinculo com: ${erros.substring(1,erros.length)}`);

        }
      }
    )
  }


}
