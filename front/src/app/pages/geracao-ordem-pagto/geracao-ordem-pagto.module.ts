import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GeracaoOrdemPagtoRoutingModule } from './geracao-ordem-pagto-routing.module';
import { GeracaoOrdemPagtoComponent } from './geracao-ordem-pagto.component';
import { NbButtonModule, NbCardModule, NbCheckboxModule, NbDatepickerModule, NbInputModule, NbSelectModule } from '@nebular/theme';
import { OrdemPagtoService } from '../../@core/services/ordem-pagto.service';
import { EstabelecimentoService } from '../../@core/services/estabelecimento.service';
import { ContaCaixaService } from '../../@core/services/conta-caixa.service';


@NgModule({
  declarations: [
    GeracaoOrdemPagtoComponent
  ],
  imports: [
    CommonModule,
    GeracaoOrdemPagtoRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbCheckboxModule,
    NbDatepickerModule
  ],
  providers: [
    {
      provide: LOCALE_ID, useValue: 'pt-BR'
    },
    OrdemPagtoService,
    EstabelecimentoService,
    ContaCaixaService
  ]
})
export class GeracaoOrdemPagtoModule { }
