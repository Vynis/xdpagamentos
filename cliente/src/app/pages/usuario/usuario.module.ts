import { ClienteService } from './../../@core/services/cliente.service';
import { UsuarioService } from './../../@core/services/usuario.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsuarioRoutingModule } from './usuario-routing.module';
import { UsuarioComponent } from './usuario.component';
import { NbAlertModule, NbButtonModule, NbCardModule, NbInputModule, NbSelectModule, NbTabsetModule, NbToastrModule, NbTooltipModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { InterceptService } from '../../@core/utils/intercept.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { SessaoService } from '../../@core/services/sessao.service';
import { EstabelecimentoService } from '../../@core/services/estabelecimento.service';
import { AlterarSenhaComponent } from './alterar-senha/alterar-senha.component';
import { UsuarioClienteService } from '../../@core/services/usuario-cliente.service';


@NgModule({
  declarations: [
    UsuarioComponent,
    AlterarSenhaComponent
  ],
  imports: [
    CommonModule,
    UsuarioRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbTabsetModule,
    ReactiveFormsModule,
    NbAlertModule,
    Ng2SmartTableModule,
    NbTooltipModule
    
  ],
  providers: [
    InterceptService,
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    UsuarioService,
    SessaoService,
    EstabelecimentoService,
    ClienteService,
    UsuarioClienteService
  ]
})
export class UsuarioModule { }
