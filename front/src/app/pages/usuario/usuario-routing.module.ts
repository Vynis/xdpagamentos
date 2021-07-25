import { UsuarioCadastroComponent } from './usuario-cadastro/usuario-cadastro.component';
import { UsuarioListaComponent } from './usuario-lista/usuario-lista.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsuarioComponent } from './usuario.component';

const routes: Routes = [
  { 
    path: '', 
    component: UsuarioComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: UsuarioListaComponent
      },
      {
        path: 'cadastro/add',
        component: UsuarioCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: UsuarioCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsuarioRoutingModule { }
