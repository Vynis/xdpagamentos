import { SessaoModel } from './../../../@core/models/sessao.model';
import { SessaoService } from './../../../@core/services/sessao.service';
import { ToastPadrao } from './../../../@core/enums/toast.enum';
import { ToastService } from './../../../@core/services/toast.service';
import { UsuarioService } from './../../../@core/services/usuario.service';
import { UsuarioModel } from './../../../@core/models/usuario.model';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { LocalDataSource } from 'ng2-smart-table';
import { AcoesPadrao } from '../../../@core/enums/acoes.enum';

@Component({
  selector: 'ngx-usuario-cadastro',
  templateUrl: './usuario-cadastro.component.html',
  styleUrls: ['./usuario-cadastro.component.scss']
})
export class UsuarioCadastroComponent implements OnInit {
  tituloPagina: string = 'Cadastro de Usuarios';
  formulario: FormGroup;
  usuarioOld: UsuarioModel;
  existeErro: boolean = false;
  ehEdicao: boolean = false;
  listaSessao: SessaoModel[];
  listaSessaoCopia: SessaoModel[];
  listaGridSessao: SessaoModel[] = [];
  
  columns = {
    id: {
      title: 'ID',
      type: 'number',
    },
    descricao: {
      title: 'Descrição',
      type: 'string',
    },
    referencia: {
      title: 'Referencia',
      type: 'string',
    },
  }

  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private usuarioService: UsuarioService,
    private sessaoService: SessaoService,
    private toastService : ToastService
  ) { }

  ngOnInit(): void {
    this.settings.columns = this.columns;
    this.settings.actions.custom = [
      { name: AcoesPadrao.REMOVER, title: '<i title="Remover" class="nb-trash"></i>'}
    ];


    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.tituloPagina = `Editar Usuário - Nº ${id}`;
        this.buscaPorId(id);
        this.ehEdicao = true;
      }
      else {
        const model = new UsuarioModel();
        this.createForm(model);
      }
    });
  }

  createForm(_usuario: UsuarioModel) {
    this.usuarioOld = Object.assign({},_usuario);
    this.buscarSessao();

    this.formulario = this.fb.group({
      id: [_usuario.id],
      nome: [_usuario.nome, Validators.required],
      cpf: [_usuario.cpf, Validators.required],
      email: [_usuario.email, [Validators.required, Validators.email]],
      status: [ _usuario.id == null ? 'A' : _usuario.status, Validators.required],
      sessao: ['']
    });
  }

  buscarSessao() {
    this.sessaoService.buscarTodos().subscribe(res => {
      if (!res.success) {
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
        console.log(res.data);
        return;
      }

      this.listaSessao = res.data;
      this.listaSessaoCopia = res.data;

      if (this.usuarioOld.listaPermissao !== undefined)
        if (this.usuarioOld.listaPermissao.length > 0)  {
          this.listaSessao = [];
          const lista = this.usuarioOld.listaPermissao;
          
          res.data.forEach(res => {
            
            const item = lista.filter(x => x.sesId == res.id);

            if (item.length == 0)
              this.listaSessao.push(res);

          });
        
      }
    },
    error => {
      console.log(error);
      this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
    }
    )
  }

  buscaPorId(id: number) {
    this.usuarioService.buscaPorId(id).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        const resUsuario = <UsuarioModel>res.data;

        this.createForm(resUsuario);

        let listaSessaoUsuario: SessaoModel[] = [];

        resUsuario.listaPermissao.forEach(res => {
          listaSessaoUsuario.push(res.sessao);
        });

        this.listaGridSessao= listaSessaoUsuario;

        this.source.load(listaSessaoUsuario);
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }

  validacao() : boolean {
    this.existeErro = false;
    const controls = this.formulario.controls;

    if (this.formulario.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro = true;
      return false;
    }

    return true;
  }

  prepararModel() : UsuarioModel {
    const controls = this.formulario.controls;
    const _usuario = new UsuarioModel();

    _usuario.id = controls.id.value == null ? 0 : controls.id.value;
    _usuario.cpf = controls.cpf.value;
    _usuario.nome = controls.nome.value;
    _usuario.email = controls.email.value;
    _usuario.status = controls.status.value;
    _usuario.senha = this.ehEdicao ? this.usuarioOld.senha : 'usu123456';

    this.listaGridSessao.forEach(res => {
      _usuario.listaPermissao.push({ id: 0, sesId: res.id, usuId: 0, sessao: null });
    });

    return _usuario;
  }

  submit() {
    if (this.validacao() === false)
      return;

    let conteudoModelPreparado = this.prepararModel();
  
    if (conteudoModelPreparado.id > 0) {
      this.alterar(conteudoModelPreparado);
      return;
    }

    this.inserir(conteudoModelPreparado);

  }

  inserir(model : UsuarioModel) {
    this.usuarioService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/usuario');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(model: UsuarioModel) {
    this.usuarioService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/usuario');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }

  voltar() {
    this.route.navigateByUrl('/pages/usuario');
  }

  onCustom(event) {
    switch (event.action) {
      case AcoesPadrao.REMOVER:
        this.remover(event.data.id);
        break;
      default:
        break;
    }
  }

  addSessao() {
      const sessao = this.formulario.controls.sessao.value;

      if (sessao === '')
        return;
      
      this.listaGridSessao.push(sessao);
      this.source.load(this.listaGridSessao);


      this.listaSessao = [];

      this.listaSessaoCopia.forEach(res => {
        const item = this.listaGridSessao.filter(x => x.id == res.id);

        if (item.length == 0)
          this.listaSessao.push(res);
      })

      this.formulario.controls.sessao.setValue(null);

  }

  addTodasSessoes() {
    this.listaSessao = [];
    this.listaGridSessao = this.listaSessaoCopia;

    this.source.load(this.listaGridSessao);
    this.formulario.controls.sessao.setValue(null);
  }

  remover(id) {
    let index = this.listaGridSessao.findIndex(x => x.id === id);

    this.listaGridSessao.splice(index,1);
    this.source.load(this.listaGridSessao);

    this.listaSessao = [];

    this.listaSessaoCopia.forEach(res => {
      const item = this.listaGridSessao.filter(x => x.id == res.id);

      if (item.length == 0)
        this.listaSessao.push(res);
    })
  }

  removerTodasSessoes() {
    this.listaSessao = this.listaSessaoCopia;
    this.listaGridSessao = [];

    this.source.load(this.listaGridSessao);
  }

}
