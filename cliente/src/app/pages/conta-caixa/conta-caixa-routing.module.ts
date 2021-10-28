import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContaCaixaCadastroComponent } from './conta-caixa-cadastro/conta-caixa-cadastro.component';
import { ContaCaixaListaComponent } from './conta-caixa-lista/conta-caixa-lista.component';
import { ContaCaixaComponent } from './conta-caixa.component';

const routes: Routes = [
  { 
    path: '', 
    component: ContaCaixaComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: ContaCaixaListaComponent
      },
      {
        path: 'cadastro/add',
        component: ContaCaixaCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: ContaCaixaCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContaCaixaRoutingModule { }
