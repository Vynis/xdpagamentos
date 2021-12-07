import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GestaoPagamentoCadastroComponent } from './gestao-pagamento-cadastro/gestao-pagamento-cadastro.component';
import { GestaoPagamentoListaComponent } from './gestao-pagamento-lista/gestao-pagamento-lista.component';
import { GestaoPagamentoComponent } from './gestao-pagamento.component';


const routes: Routes = [
  { 
    path: '', 
    component: GestaoPagamentoComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: GestaoPagamentoListaComponent
      },
      {
        path: 'cadastro/add',
        component: GestaoPagamentoCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: GestaoPagamentoCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestaoPagamentoRoutingModule { }
