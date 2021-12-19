import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestaoExtratoRoutingModule } from './gestao-extrato-routing.module';
import { GestaoExtratoComponent } from './gestao-extrato.component';
import { NbAlertModule, NbButtonModule, NbCardModule, NbDatepickerModule, NbInputModule, NbSelectModule, NbTabsetModule, NbToggleModule, NbTooltipModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { NbMomentDateModule } from '@nebular/moment';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { OrderModule } from 'ngx-order-pipe';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ClienteService } from '../../@core/services/cliente.service';
import { ContaCaixaService } from '../../@core/services/conta-caixa.service';
import { FormaPagtoService } from '../../@core/services/forma-pagto.service';
import { InterceptService } from '../../@core/utils/intercept.service';
import { GestaoExtratoListaComponent } from './gestao-extrato-lista/gestao-extrato-lista.component';
import { GestaoExtratoCadastroComponent } from './gestao-extrato-cadastro/gestao-extrato-cadastro.component';
import { GestaoExtratoService } from '../../@core/services/gestao-extrato-service';


@NgModule({
  declarations: [
    GestaoExtratoComponent,
    GestaoExtratoListaComponent,
    GestaoExtratoCadastroComponent
  ],
  imports: [
    CommonModule,
    GestaoExtratoRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbTabsetModule,
    ReactiveFormsModule,
    NbAlertModule,
    Ng2SmartTableModule,
    NbTooltipModule,
    NbToggleModule,
    OrderModule,
    NbDatepickerModule,
    NbMomentDateModule
  ],
  providers: [
    InterceptService,
    {
      provide: LOCALE_ID, useValue: 'pt-BR'
    },
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    FormaPagtoService,
    ContaCaixaService,
    GestaoExtratoService
  ]
})
export class GestaoExtratoModule { }
