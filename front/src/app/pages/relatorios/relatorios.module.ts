import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { RelatoriosComponent } from './relatorios.component';
import { RelatorioRoutingModule } from './relatorios-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NbMomentDateModule } from '@nebular/moment';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule, NbDatepickerModule, NbCheckboxModule, NbDialogModule, NbSpinnerModule } from '@nebular/theme';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { OrderModule } from 'ngx-order-pipe';
import { InterceptService } from '../../@core/utils/intercept.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RelatoriosSolicitacoesComponent } from './relatorios-solicitacoes/relatorios-solicitacoes.component';
import { RelatorioSaldoClientesComponent } from './relatorio-saldo-clientes/relatorio-saldo-clientes.component';
import { RelatorioSaldoContaCorrentesComponent } from './relatorio-saldo-conta-correntes/relatorio-saldo-conta-correntes.component';
import { RelatoriosService } from '../../@core/services/relatorios.service';
import { FormaPagtoService } from '../../@core/services/forma-pagto.service';
import localePt from '@angular/common/locales/pt';
import { ClienteService } from '../../@core/services/cliente.service';

registerLocaleData(localePt);

@NgModule({
  imports: [
    CommonModule,
    RelatorioRoutingModule,
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
    CurrencyMaskModule,
    NbCheckboxModule,
    NbDialogModule,
    NbSpinnerModule
  ],
  declarations: [
    RelatoriosComponent,
    RelatoriosSolicitacoesComponent,
    RelatorioSaldoClientesComponent, 
    RelatorioSaldoContaCorrentesComponent
  ],
  providers: [
    InterceptService,
    {
      provide: LOCALE_ID, useValue: 'pt-BR'
    },
    {
      provide:  DEFAULT_CURRENCY_CODE,
      useValue: 'BRL'
   },
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    RelatoriosService,
    FormaPagtoService,
    ClienteService
  ]
})
export class RelatoriosModule { }
