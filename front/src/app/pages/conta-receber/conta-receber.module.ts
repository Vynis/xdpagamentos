import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContaReceberComponent } from './conta-receber.component';
import { ContaReceberListaComponent } from './conta-receber-lista/conta-receber-lista.component';
import { ContaReceberCadastroComponent } from './conta-receber-cadastro/conta-receber-cadastro.component';
import { ContaReceberRoutingModule } from './conta-receber-rounting.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';

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
    NbToggleModule
  ],
  declarations: [
    ContaReceberComponent,
    ContaReceberListaComponent,
    ContaReceberCadastroComponent
  ]
})
export class ContaReceberModule { }
