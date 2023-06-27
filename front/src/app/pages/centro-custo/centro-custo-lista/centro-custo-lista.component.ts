import { Component, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { Router } from '@angular/router';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { CentroCustoService } from '../../../@core/services/centro-custo.service';
import { SweetalertService } from '../../../@core/services/sweetalert.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { SweetAlertIcons } from '../../../@core/enums/sweet-alert-icons-enum';

@Component({
  selector: 'ngx-centro-custo-lista',
  templateUrl: './centro-custo-lista.component.html',
  styleUrls: ['./centro-custo-lista.component.scss']
})
export class CentroCustoListaComponent implements OnInit {

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

  constructor(    private centroCustoService: CentroCustoService,
    private route: Router,
    private authService: AuthServiceService,
    private sweetAlertService: SweetalertService) {
      this.validaPermissao();
     }

  ngOnInit() {
  }

  private validaPermissao() {
    this.authService.validaPermissaoTela(SessoesEnum.LISTA_CENTRO_CUSTO);
    this.authService.permissaoUsuario().subscribe(res => {
      if (!res.success)
        return;

      const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_CENTRO_CUSTO);
      const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_CENTRO_CUSTO);
      this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_CENTRO_CUSTO);
      this.carregaPagina = true;

      this.buscaDados();
      this.configuracaoesGrid(validaExclusao,validaAlteracao );

    });
  }

  buscaDados() {
    this.centroCustoService.buscarTodos().subscribe(
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
        this.route.navigateByUrl(`/pages/centro-custo/cadastro/edit/${event.data.id}`);
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
    this.centroCustoService.remover(id).subscribe(
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
