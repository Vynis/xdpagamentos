import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ECommerceComponent } from './e-commerce/e-commerce.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'dashboard',
      component: ECommerceComponent,
    },
    {
      path: 'iot-dashboard',
      component: DashboardComponent,
    },
    {
      path: 'usuario',
      loadChildren: () =>  import('./usuario/usuario.module').then(m => m.UsuarioModule)
    },
    { 
      path: 'cliente',
      loadChildren: () => import('./cliente/cliente.module').then(m => m.ClienteModule) 
    },
    {
      path: 'terminal',
      loadChildren: () => import('./terminal/terminal.module').then(m => m.TerminalModule)
    },
    {
      path: 'conta',
      loadChildren: () => import('./conta-caixa/conta-caixa.module').then(m => m.ContaCaixaModule)
    },
    {
      path: 'estabelecimento',
      loadChildren: () => import('./estabelecimento/estabelecimento.module').then(m => m.EstabelecimentoModule)
    },
    {
      path: 'geracao-pagto',
      loadChildren: () => import('./geracao-ordem-pagto/geracao-ordem-pagto.module').then(m => m.GeracaoOrdemPagtoModule)
    },
    {
      path: '',
      redirectTo: 'dashboard',
      pathMatch: 'full',
    },
    {
      path: '**',
      component: NotFoundComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
