import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EstabelecimentoRoutingModule } from './estabelecimento-routing.module';
import { EstabelecimentoComponent } from './estabelecimento.component';
import { EstabelecimentoListaComponent } from './estabelecimento-lista/estabelecimento-lista.component';
import { EstabelecimentoCadastroComponent } from './estabelecimento-cadastro/estabelecimento-cadastro.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { EstabelecimentoService } from '../../@core/services/estabelecimento.service';
import { InterceptService } from '../../@core/utils/intercept.service';


@NgModule({
  declarations: [
    EstabelecimentoComponent,
    EstabelecimentoListaComponent,
    EstabelecimentoCadastroComponent
  ],
  imports: [
    CommonModule,
    EstabelecimentoRoutingModule,
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
    EstabelecimentoService
  ]
})
export class EstabelecimentoModule { }
