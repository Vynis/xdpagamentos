import { ClienteModel } from './../../../@core/models/cliente.model';
import { SessoesEnum } from './../../../@core/enums/sessoes.enum';
import { AuthServiceService } from './../../../@core/services/auth-service.service';
import { SweetalertService } from './../../../@core/services/sweetalert.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PaginationFilterModel } from './../../../@core/models/configuracao/paginationfilter.model';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalDataSource, Ng2SmartTableComponent } from 'ng2-smart-table';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { ClienteService } from '../../../@core/services/cliente.service';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { EstabelecimentoModel } from '../../../@core/models/estabelecimento.model';
import { EstabelecimentoService } from '../../../@core/services/estabelecimento.service';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { SweetAlertIcons } from '../../../@core/enums/sweet-alert-icons-enum';


@Component({
  selector: 'ngx-cliente-lista',
  templateUrl: './cliente-lista.component.html',
  styleUrls: ['./cliente-lista.component.scss']
})
export class ClienteListaComponent implements OnInit {

  @ViewChild('table') smartTable: Ng2SmartTableComponent;

  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    nome: {
      title: 'Nome',
      type: 'string',
    },
    cnpjCpfFormatado: {
      title: 'Documento',
      type: 'string',
    },
    estabelecimentoFormatado: {
      title: 'Estabelecimento',
      type: 'string',
    },
    limiteCredito: {
      title: 'Limite Crédito',
      type: 'string',
    },
    status: {
      title: 'Status',
      type: 'string',
    }
  }

  columnsAG = {
    id: {
      title: 'ID',
      type: 'number',
    },
    nome: {
      title: 'Nome',
      type: 'string',
    },
    nomeAgrupamento: {
      title: 'Grupo',
      type: 'string',
    },
    cnpjCpfFormatado: {
      title: 'Documento',
      type: 'string',
    },
    estabelecimentoFormatado: {
      title: 'Estabelecimento',
      type: 'string',
    },
    limiteCredito: {
      title: 'Limite Crédito',
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
  listaEstabelecimentos: EstabelecimentoModel[];
  validaNovo: boolean = false;
  carregaPagina: boolean = false;
  agrupamento: boolean = false;
  selectedRows: ClienteModel[] = [];
  listaGrupos: any[];

  constructor(
    private clienteService: ClienteService,
    private estabelecimentoService: EstabelecimentoService,
    private toastService : ToastService,
    private route: Router,
    private fb: FormBuilder,
    private sweetAlertService: SweetalertService,
    private authService: AuthServiceService,
    private activatedRoute: ActivatedRoute
    ) {

    this.validaPermissao();
    this.buscarListaEstabelecimentos();
   }

  private validaPermissao() {
    this.authService.validaPermissaoTela(SessoesEnum.LISTA_CLIENTES);
    this.authService.permissaoUsuario().subscribe(res => {
      if (!res.success)
        return;

      const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_CLIENTES);
      const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_CLIENTES);
      const validaAlteracaoSenha = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_SENHA_CLIENTES);
      this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_CLIENTES);
      this.carregaPagina = true;

      this.buscaDados(new PaginationFilterModel);
      this.configuracaoesGrid(validaExclusao,validaAlteracao,validaAlteracaoSenha );

    });
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe(params => {
      const param = params.agrupamento;
      this.agrupamento = param == 'AG' ? true : false;
    });

    if (this.agrupamento)
      this.buscarListaGrupos();

    this.createForm();
  }

  createForm() {
    this.formulario = this.fb.group({
      tipo: ['0'],
      descricao: [''],
      estabelecimento: ['0'],
      status: ['0'],
      nomeAgrupamento: [''],
      filtroGrupo: ['0'],
    })
  }

  configuracaoesGrid(validaExclusao: boolean = false, validaAlteracao: boolean = false, validaAlteracaoSenha: boolean = false){
    this.settings.selectMode = this.agrupamento ? 'multi' : '';
    this.settings.columns = this.agrupamento ? this.columnsAG : this.columns;
    this.settings.actions.custom = [];

    if (validaAlteracao)
      this.settings.actions.custom.push({ name: AcoesPadrao.EDITAR, title: '<i title="Editar" class="nb-edit"></i>'});

    if (validaExclusao)
      this.settings.actions.custom.push({ name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'});

    if (validaAlteracaoSenha)
      this.settings.actions.custom.push({ name: 'Senha', title: '<i title="Alterar Senha" class="nb-arrow-retweet"></i>' });
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

  buscarListaEstabelecimentos() {
    this.estabelecimentoService.buscarAtivos().subscribe(
      res => {
        
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaEstabelecimentos = res.data;

      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
      
    )
  }

  buscarListaGrupos() {
    this.clienteService.buscarGrupo().subscribe(
      res => {
        
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaGrupos = res.data;

      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
      
    )
  }

  onCustom(event) {
    switch (event.action) {
      case AcoesPadrao.EDITAR:
        this.route.navigateByUrl(`/pages/cliente/cadastro/edit/${event.data.id}`);
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
      case 'Senha':
        this.sweetAlertService.msgPadrao('Tem certeza que deseja alterar a senha?', 'Será alterado para senha: cli102030', SweetAlertIcons.QUESTION).then(
          res => {
            if (res.isConfirmed) {
              this.clienteService.alterarSenhaPadrao(event.data.id).subscribe(
                res => {
                  if (res.success) 
                   this.sweetAlertService.msgAvulsa('Senha alterada com sucesso!', SweetAlertIcons.SUCESS, '');
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

    if (controls.estabelecimento.value !== '0') {
      var item  = new FiltroItemModel();
      item.property = 'EstId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.estabelecimento.value;
      listaItem.push(item);
    }

    if (controls.status.value !== '0') {
      var item  = new FiltroItemModel();
      item.property = 'Status';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.status.value;
      listaItem.push(item);
    }

    if (this.agrupamento) {
      if (controls.filtroGrupo.value !== '0') {
        var item  = new FiltroItemModel();
        item.property = 'NomeAgrupamento';
        item.filterType = FilterTypeConstants.EQUALS;
        item.value = controls.filtroGrupo.value;
        listaItem.push(item);
      }
    }

    filtro.filtro = listaItem;
    this.buscaDados(filtro);

  }

  agrupar() {

    const nomeAgrupamento  = this.formulario.controls.nomeAgrupamento.value;

    if (nomeAgrupamento == null || nomeAgrupamento == '') {
      this.toastService.showToast(ToastPadrao.DANGER, 'Informe o nome do agrupamento');
      return;
    }

    if (this.selectedRows.length === 0) {
      this.toastService.showToast(ToastPadrao.DANGER, 'Selecione um cliente');
      return;
    }


    this.selectedRows.forEach(res => {
      res.nomeAgrupamento = nomeAgrupamento;
    })

    this.clienteService.agrupar(this.selectedRows).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao agrupar');
          console.log(res.data);
          return;
        }


        this.toastService.showToast(ToastPadrao.SUCCESS, 'Agrupamento realizado com sucesso!');

        this.pesquisar();
      }
    )
  }

  desagrupar() {

    if (this.selectedRows.length === 0) {
      this.toastService.showToast(ToastPadrao.DANGER, 'Selecione um cliente');
      return;
    }

    this.clienteService.desagrupar(this.selectedRows).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao agrupar');
          console.log(res.data);
          return;
        }


        this.toastService.showToast(ToastPadrao.SUCCESS, 'Desagrupamento realizado com sucesso!');

        this.pesquisar();
      }
    )

  }

  onRowSelect(event) {
    this.selectedRows = event.selected;
  }

  deletar(id: number) {
    this.clienteService.remover(id).subscribe(
      res => {
        if (res.success) {
          this.sweetAlertService.msgAvulsa('Deletado', SweetAlertIcons.SUCESS ,'');
          this.pesquisar();
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
