import { RelatoriosComponent } from './relatorios.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RelatoriosSolicitacoesComponent } from './relatorios-solicitacoes/relatorios-solicitacoes.component';
import { RelatorioSaldoClientesComponent } from './relatorio-saldo-clientes/relatorio-saldo-clientes.component';
import { RelatorioSaldoContaCorrentesComponent } from './relatorio-saldo-conta-correntes/relatorio-saldo-conta-correntes.component';
import { RelatorioGeralVendasComponent } from './relatorio-geral-vendas/relatorio-geral-vendas.component';


const routes: Routes = [
  { 
    path: '', 
    component: RelatoriosComponent,
    children: [
      { 
        path: '',
        redirectTo: 'solicitacoes',
        pathMatch: 'full'
      },
      { 
        path: 'solicitacoes', 
        component: RelatoriosSolicitacoesComponent
      },
      {
        path: 'saldo-clientes',
        component: RelatorioSaldoClientesComponent
      },
      {
        path: 'saldo-conta-correntes',
        component: RelatorioSaldoContaCorrentesComponent
      },
      {
        path: 'geral-vendas',
        component: RelatorioGeralVendasComponent
      }
    ]   
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RelatorioRoutingModule { }