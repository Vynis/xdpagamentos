import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { ContaReceberComponent } from './conta-receber.component';
import { ContaReceberListaComponent } from './conta-receber-lista/conta-receber-lista.component';
import { ContaReceberCadastroComponent } from './conta-receber-cadastro/conta-receber-cadastro.component';
import { ContaReceberRoutingModule } from './conta-receber-rounting.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule, NbDatepickerModule, NbRadioModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CentroCustoService } from '../../@core/services/centro-custo.service';
import { ContaReceberService } from '../../@core/services/conta-receber.service';
import { InterceptService } from '../../@core/utils/intercept.service';
import { NbMomentDateModule } from '@nebular/moment';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ContaReceberBaixaComponent } from './conta-receber-baixa/conta-receber-baixa.component';
import { ContaCaixaService } from '../../@core/services/conta-caixa.service';
import { FluxoCaixaService } from '../../@core/services/fluxo-caixa.service';
import { PlanoContaService } from '../../@core/services/plano-conta.service';
import localePt from '@angular/common/locales/pt';
import { ContaReceberItensFluxoCaixaComponent } from './conta-receber-itens-fluxo-caixa/conta-receber-itens-fluxo-caixa.component';

registerLocaleData(localePt);

@NgModule({
  imports: [
    CommonModule,
    ContaReceberRoutingModule,
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
    NbDatepickerModule,
    NbMomentDateModule,
    CurrencyMaskModule,
    NbRadioModule
  ],
  declarations: [
    ContaReceberComponent,
    ContaReceberListaComponent,
    ContaReceberCadastroComponent,
    ContaReceberBaixaComponent,
    ContaReceberItensFluxoCaixaComponent
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
    ContaReceberService,
    CentroCustoService,
    PlanoContaService,
    ContaCaixaService,
    FluxoCaixaService
  ]
})
export class ContaReceberModule { }
