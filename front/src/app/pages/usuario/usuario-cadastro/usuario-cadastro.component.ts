import { ToastPadrao } from './../../../@core/enums/toast.enum';
import { ToastService } from './../../../@core/services/toast.service';
import { UsuarioService } from './../../../@core/services/usuario.service';
import { UsuarioModel } from './../../../@core/models/usuario.model';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

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

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private usuarioService: UsuarioService,
    private toastService : ToastService
  ) { }

  ngOnInit(): void {
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

    this.formulario = this.fb.group({
      id: [_usuario.id],
      nome: [_usuario.nome, Validators.required],
      cpf: [_usuario.cpf, Validators.required],
      email: [_usuario.email, [Validators.required, Validators.email]],
      status: [ _usuario.id == null ? 'A' : _usuario.status, Validators.required]
    });
  }

  buscaPorId(id: number) {
    this.usuarioService.buscaPorId(id).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.createForm(res.data);
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

}
