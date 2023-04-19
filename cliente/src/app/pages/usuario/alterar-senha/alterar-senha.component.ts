import { ClienteService } from './../../../@core/services/cliente.service';
import { NbAuthJWTToken, NbAuthService } from '@nebular/auth';
import { UsuarioService } from './../../../@core/services/usuario.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { ConfirmPasswordValidator } from '../../../@core/utils/confirm-password.validator';
import { UsuarioClienteService } from '../../../@core/services/usuario-cliente.service';

@Component({
  selector: 'ngx-alterar-senha',
  templateUrl: './alterar-senha.component.html',
  styleUrls: ['./alterar-senha.component.scss']
})
export class AlterarSenhaComponent implements OnInit {

  existeErro: boolean = false;
  formulario: FormGroup;
  idUsuario: string = '';

  constructor(
    protected ref: NbDialogRef<AlterarSenhaComponent>,
    private fb: FormBuilder,
    private toastService : ToastService,
    private authService : NbAuthService,
    private usuarioClienteService: UsuarioClienteService
    ) {
    
      this.authService.onTokenChange().subscribe(
        (token: NbAuthJWTToken) => {
          if (token.isValid()) { 
            const payload = token.getPayload();
            this.idUsuario = payload.unique_name;
          }        
        }
      )

   }

  ngOnInit(): void {
   this.createForm();
  }

  cancel() {
   this.ref.close();
  }

  createForm() {
    this.formulario = this.fb.group({
      senhaAtual: ['', Validators.required],
      senhaNovo: ['', Validators.required],
      confirmarSenha: ['', Validators.required] 
    },
     {
      validator: ConfirmPasswordValidator.MatchPassword
    });
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

  submit() { 

    if (this.validacao() === false)
      return;

    const control = this.formulario.controls;

    this.usuarioClienteService.alterarSenha({ idUsuario: this.idUsuario, senhaAtual: control.senhaAtual.value, senhaNova: control.senhaNovo.value }).subscribe(
      res => {

        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração', res.data);
          return;
        }

         this.toastService.showToast(ToastPadrao.SUCCESS, 'Ateração de senha realizado com sucesso!');
         this.ref.close();
        
      }
    )

  }



}
