import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'dashboard',
      component: DashboardComponent,
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
      path: 'extrato', 
      loadChildren: () => import('./extrato/extrato.module').then(m => m.ExtratoModule) 
    },
    {
      path: 'pagamento',
      loadChildren: () => import('./pagamento/pagamento.module').then(m => m.PagamentoModule)
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
