import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GestaoExtratoCadastroComponent } from './gestao-extrato-cadastro/gestao-extrato-cadastro.component';
import { GestaoExtratoListaComponent } from './gestao-extrato-lista/gestao-extrato-lista.component';
import { GestaoExtratoComponent } from './gestao-extrato.component';

const routes: Routes = [
  { 
    path: '', 
    component: GestaoExtratoComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: GestaoExtratoListaComponent
      },
      {
        path: 'cadastro/add',
        component: GestaoExtratoCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: GestaoExtratoCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestaoExtratoRoutingModule { }
