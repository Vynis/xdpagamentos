import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExtratoListaComponent } from './extrato-lista/extrato-lista.component';
import { ExtratoOperacaoComponent } from './extrato-operacao/extrato-operacao.component';
import { ExtratoComponent } from './extrato.component';

const routes: Routes = [
  { 
    path: '', 
    component: ExtratoComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: ExtratoListaComponent
      },
      { 
        path: 'operacao', 
        component: ExtratoOperacaoComponent
      },
    ]
  }
];
//TEste
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExtratoRoutingModule { }
