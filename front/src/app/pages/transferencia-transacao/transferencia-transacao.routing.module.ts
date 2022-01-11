import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransferenciaTransacaoComponent } from './transferencia-transacao.component';


const routes: Routes = [{ path: '', component: TransferenciaTransacaoComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransferenciaTransacaoRoutingModule { }
