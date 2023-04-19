import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UsuarioClienteCadastroComponent } from "./usuario-cliente-cadastro/usuario-cliente-cadastro.component";
import { UsuarioClienteListaComponent } from "./usuario-cliente-lista/usuario-cliente-lista.component";
import { UsuarioClienteComponent } from "./usuario-cliente.component";

const routes: Routes = [
    { 
      path: '', 
      component: UsuarioClienteComponent,
      children: [
        { 
          path: '',
          redirectTo: 'lista',
          pathMatch: 'full'
        },
        { 
          path: 'lista', 
          component: UsuarioClienteListaComponent
        },
        {
          path: 'cadastro/add',
          component: UsuarioClienteCadastroComponent
        },
        {
          path: 'cadastro/edit/:id',
          component: UsuarioClienteCadastroComponent
        }
      ]
    }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class UsuarioClienteRoutingModule { }