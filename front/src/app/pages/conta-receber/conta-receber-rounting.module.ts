import { RouterModule, Routes } from "@angular/router";
import { ContaReceberComponent } from "./conta-receber.component";
import { ContaReceberListaComponent } from "./conta-receber-lista/conta-receber-lista.component";
import { ContaReceberCadastroComponent } from "./conta-receber-cadastro/conta-receber-cadastro.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    { 
      path: '', 
      component: ContaReceberComponent,
      children: [
        { 
          path: '',
          redirectTo: 'lista',
          pathMatch: 'full'
        },
        { 
          path: 'lista', 
          component: ContaReceberListaComponent
        },
        {
          path: 'cadastro/add',
          component: ContaReceberCadastroComponent
        },
        {
          path: 'cadastro/edit/:id',
          component: ContaReceberCadastroComponent
        },
      ]
    }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class ContaReceberRoutingModule { }