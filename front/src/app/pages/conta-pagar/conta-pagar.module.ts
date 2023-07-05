import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { ContaPagarComponent } from './conta-pagar.component';
import { ContaPagarListaComponent } from './conta-pagar-lista/conta-pagar-lista.component';
import { ContaPagarCadastroComponent } from './conta-pagar-cadastro/conta-pagar-cadastro.component';
import { ContaPagarRoutingModule } from './conta-pagar-rounting.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule, NbDatepickerModule, NbRadioModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { NbMomentDateModule } from '@nebular/moment';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { OrderModule } from 'ngx-order-pipe';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { InterceptService } from '../../@core/utils/intercept.service';
import { ContaPagarService } from '../../@core/services/conta-pagar.service';
import { CentroCustoService } from '../../@core/services/centro-custo.service';
import { ContaPagarBaixaComponent } from './conta-pagar-baixa/conta-pagar-baixa.component';
import { PlanoContaService } from '../../@core/services/plano-conta.service';
import localePt from '@angular/common/locales/pt';
import { ContaCaixaService } from '../../@core/services/conta-caixa.service';
import { FluxoCaixaService } from '../../@core/services/fluxo-caixa.service';
import { ContaPagarItensFluxoCaixaComponent } from './conta-pagar-itens-fluxo-caixa/conta-pagar-itens-fluxo-caixa.component';

registerLocaleData(localePt);


@NgModule({
  imports: [
    CommonModule,
    ContaPagarRoutingModule,
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
    NbRadioModule
  ],
  declarations: [
    ContaPagarComponent,
    ContaPagarListaComponent,
    ContaPagarCadastroComponent,
    ContaPagarBaixaComponent,
    ContaPagarItensFluxoCaixaComponent
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
    ContaPagarService,
    CentroCustoService,
    PlanoContaService,
    ContaCaixaService,
    FluxoCaixaService
  ]
})
export class ContaPagarModule { }
