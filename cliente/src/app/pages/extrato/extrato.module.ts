import { GestaoPagamentoService } from './../../@core/services/gestao-pagamento-service';
import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExtratoRoutingModule } from './extrato-routing.module';
import { ExtratoComponent } from './extrato.component';
import { ExtratoListaComponent } from './extrato-lista/extrato-lista.component';
import { ExtratoOperacaoComponent } from './extrato-operacao/extrato-operacao.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule, NbDatepickerModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { OrderModule } from 'ngx-order-pipe';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { InterceptService } from '../../@core/utils/intercept.service';
import { NbMomentDateModule } from '@nebular/moment';
import { ClienteService } from '../../@core/services/cliente.service';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { FormaPagtoService } from '../../@core/services/forma-pagto.service';
import { TerminalService } from '../../@core/services/terminal.service';


@NgModule({
  declarations: [
    ExtratoComponent,
    ExtratoListaComponent,
    ExtratoOperacaoComponent
  ],
  imports: [
    CommonModule,
    ExtratoRoutingModule,
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
    NbMomentDateModule,
    CurrencyMaskModule
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
    GestaoPagamentoService,
    ClienteService,
    FormaPagtoService,
    TerminalService
  ]
})
export class ExtratoModule { }
