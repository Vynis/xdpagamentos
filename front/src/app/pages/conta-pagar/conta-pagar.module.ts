import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContaPagarComponent } from './conta-pagar.component';
import { ContaPagarListaComponent } from './conta-pagar-lista/conta-pagar-lista.component';
import { ContaPagarCadastroComponent } from './conta-pagar-cadastro/conta-pagar-cadastro.component';
import { ContaPagarRoutingModule } from './conta-pagar-rounting.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule, NbToggleModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';

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
    NbToggleModule
  ],
  declarations: [
    ContaPagarComponent,
    ContaPagarListaComponent,
    ContaPagarCadastroComponent
  ]
})
export class ContaPagarModule { }
