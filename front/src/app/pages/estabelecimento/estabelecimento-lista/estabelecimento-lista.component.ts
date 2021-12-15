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
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { SweetalertService } from '../../../@core/services/sweetalert.service';

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
  carregaPagina: boolean = false;
  validaNovo: boolean = false;

  constructor(
    private estabelecimentoService: EstabelecimentoService,
    private route: Router,
    private fb: FormBuilder,
    private authService: AuthServiceService
  ) { 
      this.validaPermissao();
  }

  ngOnInit(): void {
    this.createForm();
  }

  private validaPermissao() {
    this.authService.validaPermissaoTela(SessoesEnum.LISTA_ESTABELECIMENTO);
    this.authService.permissaoUsuario().subscribe(res => {
      if (!res.success)
        return;

      const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_ESTABELECIMENTO);
      const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_ESTABELECIMENTO);
      this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_ESTABELECIMENTO);
      this.carregaPagina = true;

      this.buscaDados(new PaginationFilterModel);
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
