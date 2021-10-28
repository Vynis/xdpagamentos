import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NgxAuthRoutingModule } from './auth-routing.module';
import { NbAuthModule } from '@nebular/auth';
import { NbAlertModule, NbButtonModule, NbCardModule, NbCheckboxModule, NbIconModule, NbInputModule } from '@nebular/theme';

import { NgxLoginComponent } from './login/login.component';
import { AlterarSenhaComponent } from '../pages/usuario/alterar-senha/alterar-senha.component'; 


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    NbAlertModule,
    NbInputModule,
    NbButtonModule,
    NbCheckboxModule,
    NgxAuthRoutingModule,
    NbIconModule,
    NbAuthModule
  ],
  declarations: [
    NgxLoginComponent
  ],
})
export class NgxAuthModule {
}