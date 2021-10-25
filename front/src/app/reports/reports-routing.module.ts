import { PreVendaComponent } from './pre-venda/pre-venda.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportsComponent } from './reports.component';

const routes: Routes = [
  { 
    path: '', 
    component: ReportsComponent,
    children: [
      {
        path: 'pre-venda',
        component: PreVendaComponent
      },
      {
        path: '',
        redirectTo: 'pre-venda',
        pathMatch: 'full',
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
