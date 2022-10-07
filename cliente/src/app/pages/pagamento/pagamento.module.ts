import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagamentoComponent } from './pagamento.component';
import { RouterModule } from '@angular/router';
import { NbAlertModule, NbButtonModule, NbCardModule, NbDatepickerModule, NbInputModule, NbSelectModule, NbTabsetModule, NbToggleModule, NbTooltipModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { OrderModule } from 'ngx-order-pipe';
import { NbMomentDateModule } from '@nebular/moment';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { InterceptService } from '../../@core/utils/intercept.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { BancoService } from '../../@core/services/banco.service';
import { ClienteService } from '../../@core/services/cliente.service';


@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: PagamentoComponent }]),
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
  declarations: [PagamentoComponent],
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
    BancoService,
    ClienteService,

  ]
})
export class PagamentoModule { }
