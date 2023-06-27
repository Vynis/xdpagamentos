import { RouterModule, Routes } from "@angular/router";
import { CentroCustoComponent } from "./centro-custo.component";
import { CentroCustoListaComponent } from "./centro-custo-lista/centro-custo-lista.component";
import { CentroCustoCadastroComponent } from "./centro-custo-cadastro/centro-custo-cadastro.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    { 
      path: '', 
      component: CentroCustoComponent,
      children: [
        { 
          path: '',
          redirectTo: 'lista',
          pathMatch: 'full'
        },
        { 
          path: 'lista', 
          component: CentroCustoListaComponent
        },
        {
          path: 'cadastro/add',
          component: CentroCustoCadastroComponent
        },
        {
          path: 'cadastro/edit/:id',
          component: CentroCustoCadastroComponent
        },
      ]
    }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class CentroCustoRoutingModule { }