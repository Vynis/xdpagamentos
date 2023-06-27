import { RouterModule, Routes } from "@angular/router";
import { ContaPagarComponent } from "./conta-pagar.component";
import { ContaPagarListaComponent } from "./conta-pagar-lista/conta-pagar-lista.component";
import { ContaPagarCadastroComponent } from "./conta-pagar-cadastro/conta-pagar-cadastro.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    { 
      path: '', 
      component: ContaPagarComponent,
      children: [
        { 
          path: '',
          redirectTo: 'lista',
          pathMatch: 'full'
        },
        { 
          path: 'lista', 
          component: ContaPagarListaComponent
        },
        {
          path: 'cadastro/add',
          component: ContaPagarCadastroComponent
        },
        {
          path: 'cadastro/edit/:id',
          component: ContaPagarCadastroComponent
        },
      ]
    }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class ContaPagarRoutingModule { }