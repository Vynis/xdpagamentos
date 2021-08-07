import { ClienteCadastroComponent } from './cliente-cadastro/cliente-cadastro.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClienteListaComponent } from './cliente-lista/cliente-lista.component';
import { ClienteComponent } from './cliente.component';

const routes: Routes = [
  { 
    path: '', 
    component: ClienteComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: ClienteListaComponent
      },
      {
        path: 'cadastro/add',
        component: ClienteCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: ClienteCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClienteRoutingModule { }
