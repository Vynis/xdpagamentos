
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsuarioComponent } from './usuario.component';
import { AlterarSenhaComponent } from './alterar-senha/alterar-senha.component';

const routes: Routes = [
  { 
    path: '', 
    component: UsuarioComponent,
    children: [
      { 
        path: '',
        redirectTo: 'alterar-senha',
        pathMatch: 'full'
      },
      { 
        path: 'alterar-senha', 
        component: AlterarSenhaComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsuarioRoutingModule { }
