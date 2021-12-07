import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestaoPagamentoRoutingModule } from './gestao-pagamento-routing.module';
import { GestaoPagamentoComponent } from './gestao-pagamento.component';
import { GestaoPagamentoCadastroComponent } from './gestao-pagamento-cadastro/gestao-pagamento-cadastro.component';
import { GestaoPagamentoListaComponent } from './gestao-pagamento-lista/gestao-pagamento-lista.component';
import { NbAlertModule, NbButtonModule, NbCardModule, NbDatepickerModule, NbInputModule, NbSelectModule, NbTabsetModule, NbToggleModule, NbTooltipModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { OrderModule } from 'ngx-order-pipe';
import { ClienteService } from '../../@core/services/cliente.service';
import { GestaoPagamentoService } from '../../@core/services/gestao-pagamento-service';
import { InterceptService } from '../../@core/utils/intercept.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';


@NgModule({
  declarations: [
    GestaoPagamentoComponent,
    GestaoPagamentoCadastroComponent,
    GestaoPagamentoListaComponent
  ],
  imports: [
    CommonModule,
    GestaoPagamentoRoutingModule,
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
    NbDatepickerModule
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
    ClienteService,
    GestaoPagamentoService
  ]
})
export class GestaoPagamentoModule { }
