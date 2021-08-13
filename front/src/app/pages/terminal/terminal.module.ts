import { EstabelecimentoService } from './../../@core/services/estabelecimento.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TerminalRoutingModule } from './terminal-routing.module';
import { TerminalComponent } from './terminal.component';
import { TerminalListaComponent } from './terminal-lista/terminal-lista.component';
import { TerminalCadastroComponent } from './terminal-cadastro/terminal-cadastro.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { InterceptService } from '../../@core/utils/intercept.service';
import { TerminalService } from '../../@core/services/terminal.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';


@NgModule({
  declarations: [
    TerminalComponent,
    TerminalListaComponent,
    TerminalCadastroComponent
  ],
  imports: [
    CommonModule,
    TerminalRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbTabsetModule,
    ReactiveFormsModule,
    NbAlertModule,
    Ng2SmartTableModule,
    NbTooltipModule
  ],
  providers: [
    InterceptService,
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    TerminalService,
    EstabelecimentoService
  ]
})
export class TerminalModule { }
