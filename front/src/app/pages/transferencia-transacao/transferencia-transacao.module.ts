import { ClienteService } from './../../@core/services/cliente.service';
import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransferenciaTransacaoComponent } from './transferencia-transacao.component';
import { NbAlertModule, NbButtonModule, NbCardModule, NbCheckboxModule, NbDatepickerModule, NbInputModule, NbSelectModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { ContaCaixaService } from '../../@core/services/conta-caixa.service';
import { EstabelecimentoService } from '../../@core/services/estabelecimento.service';
import { TransferenciaTransacaoRoutingModule } from './transferencia-transacao.routing.module';
import { TransferenciaPagtoService } from '../../@core/services/transferencia-pago.service';

@NgModule({
  imports: [
    CommonModule,
    TransferenciaTransacaoRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbCheckboxModule,
    NbDatepickerModule,
    ReactiveFormsModule,
    NbAlertModule
  ],
  providers: [
    {
      provide: LOCALE_ID, useValue: 'pt-BR'
    },
    TransferenciaPagtoService,
    EstabelecimentoService,
    ContaCaixaService,
    ClienteService
  ],
  declarations: [TransferenciaTransacaoComponent]
})
export class TransferenciaTransacaoModule { }
