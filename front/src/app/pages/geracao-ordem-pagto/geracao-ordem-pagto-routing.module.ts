import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GeracaoOrdemPagtoComponent } from './geracao-ordem-pagto.component';

const routes: Routes = [{ path: '', component: GeracaoOrdemPagtoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GeracaoOrdemPagtoRoutingModule { }
