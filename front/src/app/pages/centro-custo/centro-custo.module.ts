import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CentroCustoComponent } from './centro-custo.component';
import { CentroCustoCadastroComponent } from './centro-custo-cadastro/centro-custo-cadastro.component';
import { CentroCustoListaComponent } from './centro-custo-lista/centro-custo-lista.component';
import { CentroCustoRoutingModule } from './centro-custo-rounting.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { InterceptService } from '../../@core/utils/intercept.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CentroCustoService } from '../../@core/services/centro-custo.service';

@NgModule({
  imports: [
    CommonModule,
    CentroCustoRoutingModule,
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
  declarations: [
    CentroCustoComponent,
    CentroCustoCadastroComponent,
    CentroCustoListaComponent
  ],
  providers: [
    InterceptService,
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    CentroCustoService
  ]
})
export class CentroCustoModule { }
