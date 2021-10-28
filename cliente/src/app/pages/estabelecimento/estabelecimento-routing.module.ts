import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EstabelecimentoCadastroComponent } from './estabelecimento-cadastro/estabelecimento-cadastro.component';
import { EstabelecimentoListaComponent } from './estabelecimento-lista/estabelecimento-lista.component';
import { EstabelecimentoComponent } from './estabelecimento.component';

const routes: Routes = [
  { 
    path: '', 
    component: EstabelecimentoComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: EstabelecimentoListaComponent
      },
      {
        path: 'cadastro/add',
        component: EstabelecimentoCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: EstabelecimentoCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EstabelecimentoRoutingModule { }
