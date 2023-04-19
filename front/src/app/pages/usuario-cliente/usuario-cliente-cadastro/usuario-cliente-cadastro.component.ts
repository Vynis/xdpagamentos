import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsuarioClienteModel } from '../../../@core/models/usuario-cliente.model';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { ToastService } from '../../../@core/services/toast.service';
import { UsuarioClienteService } from '../../../@core/services/usuario-cliente.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { ToastPadrao } from '../../../@core/enums/toast.enum';

@Component({
  selector: 'ngx-usuario-cliente-cadastro',
  templateUrl: './usuario-cliente-cadastro.component.html',
  styleUrls: ['./usuario-cliente-cadastro.component.scss']
})
export class UsuarioClienteCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Usuarios';
  formulario: FormGroup;
  usuarioOld: UsuarioClienteModel;
  existeErro: boolean = false;
  ehEdicao: boolean = false;

  constructor(    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private usuarioClienteService: UsuarioClienteService,
    private toastService : ToastService,
    private authService: AuthServiceService) { }

    ngOnInit(): void {

  
      this.activatedRoute.params.subscribe(params => {
        const id = params.id;
        if (id && id > 0) {
          this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_USUARIOS_CLIENTES);
          this.tituloPagina = `Editar Usuário - Nº ${id}`;
          this.buscaPorId(id);
          this.ehEdicao = true;
        }
        else {
          this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_USUARIOS_CLIENTES);
          const model = new UsuarioClienteModel();
          this.createForm(model);
        }
      });
    }


    createForm(_usuario: UsuarioClienteModel) {
      this.usuarioOld = Object.assign({},_usuario);
  
      this.formulario = this.fb.group({
        id: [_usuario.id],
        nome: [_usuario.nome, Validators.required],
        email: [_usuario.email, [Validators.required, Validators.email]],
        status: [ _usuario.id == null ? 'A' : _usuario.status, Validators.required]
      });
    }

    buscaPorId(id: number) {
      this.usuarioClienteService.buscaPorId(id).subscribe(
        res => {
          if (!res.success) {
            this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
            console.log(res.data);
            return;
          }
  
          const resUsuario = <UsuarioClienteModel>res.data;
  
          this.createForm(resUsuario);
  
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

    prepararModel() : UsuarioClienteModel {
      const controls = this.formulario.controls;
      const _usuario = new UsuarioClienteModel();
  
      _usuario.id = controls.id.value == null ? 0 : controls.id.value;
      _usuario.nome = controls.nome.value;
      _usuario.email = controls.email.value;
      _usuario.status = controls.status.value;
      _usuario.senha = this.ehEdicao ? this.usuarioOld.senha : 'usu123456';
  
      return _usuario;
    }

    inserir(model : UsuarioClienteModel) {
      this.usuarioClienteService.inserir(model).subscribe(
        res => {
          if (!res.success) {
            this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
            return;
          }
  
          this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
          this.route.navigateByUrl('/pages/usuario-cliente');
        },
        erro => {
          console.log(erro);
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
        }
      )
    }
  
    alterar(model: UsuarioClienteModel) {
      this.usuarioClienteService.alterar(model).subscribe(
        res => {
          if (!res.success) {
            this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
            return;
          }
  
          this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
          this.route.navigateByUrl('/pages/usuario-cliente');
        },
        erro => {
          console.log(erro);
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
        }
      )
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

    voltar() {
      this.route.navigateByUrl('/pages/usuario-cliente');
    }
  

}
