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
      component: DashboardComponent,
    },
    // {
    //   path: 'iot-dashboard',
    //   component: DashboardComponent,
    // },
    {
      path: 'usuario',
      loadChildren: () =>  import('./usuario/usuario.module').then(m => m.UsuarioModule)
    },
    {
      path: 'usuario-cliente',
      loadChildren: () =>  import('./usuario-cliente/usuario-cliente.module').then(m => m.UsuarioClienteModule)
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
      path: 'gestao-pagto',
      loadChildren: () => import('./gestao-pagamento/gestao-pagamento.module').then(m => m.GestaoPagamentoModule)
    },
    { 
      path: 'gestao-extrato', 
      loadChildren: () => import('./gestao-extrato/gestao-extrato.module').then(m => m.GestaoExtratoModule) 
    },
    { 
      path: 'transferencia-transacao', 
      loadChildren: () => import('./transferencia-transacao/transferencia-transacao.module').then(m => m.TransferenciaTransacaoModule) 
    },
    {
      path: 'relatorio',
      loadChildren: () => import('./relatorios/relatorios.module').then(m => m.RelatoriosModule)
    },
    {
      path: 'miscellaneous',
      loadChildren: () => import('./miscellaneous/miscellaneous.module').then(m => m.MiscellaneousModule)
    },
    {
      path: 'centro-custo',
      loadChildren: () => import('./centro-custo/centro-custo.module').then(m => m.CentroCustoModule)
    },
    {
      path: 'conta-pagar',
      loadChildren: () => import('./conta-pagar/conta-pagar.module').then(m => m.ContaPagarModule)
    },
    {
      path: 'conta-receber',
      loadChildren: () => import('./conta-receber/conta-receber.module').then(m => m.ContaReceberModule)
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
