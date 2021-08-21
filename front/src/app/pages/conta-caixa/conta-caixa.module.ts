import { ContaCaixaService } from './../../@core/services/conta-caixa.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContaCaixaRoutingModule } from './conta-caixa-routing.module';
import { ContaCaixaComponent } from './conta-caixa.component';
import { ContaCaixaListaComponent } from './conta-caixa-lista/conta-caixa-lista.component';
import { ContaCaixaCadastroComponent } from './conta-caixa-cadastro/conta-caixa-cadastro.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { InterceptService } from '../../@core/utils/intercept.service';


@NgModule({
  declarations: [
    ContaCaixaComponent,
    ContaCaixaListaComponent,
    ContaCaixaCadastroComponent
  ],
  imports: [
    CommonModule,
    ContaCaixaRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbTabsetModule,
    ReactiveFormsModule,
    NbAlertModule,
    Ng2SmartTableModule,
    NbTooltipModule,
    NbToggleModule
  ],
  providers: [
    InterceptService,
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    ContaCaixaService
  ]
})
export class ContaCaixaModule { }
