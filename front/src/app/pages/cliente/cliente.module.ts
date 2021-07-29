import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClienteRoutingModule } from './cliente-routing.module';
import { ClienteComponent } from './cliente.component';
import { NbAlertModule, NbButtonModule, NbCardModule, NbInputModule, NbSelectModule, NbTabsetModule, NbTooltipModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ClienteListaComponent } from './cliente-lista/cliente-lista.component';
import { ClienteCadastroComponent } from './cliente-cadastro/cliente-cadastro.component';


@NgModule({
  declarations: [
    ClienteComponent,
    ClienteListaComponent,
    ClienteCadastroComponent
  ],
  imports: [
    CommonModule,
    ClienteRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbTabsetModule,
    ReactiveFormsModule,
    NbAlertModule,
    Ng2SmartTableModule,
    NbTooltipModule
  ]
})
export class ClienteModule { }
