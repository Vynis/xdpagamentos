import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TerminalCadastroComponent } from './terminal-cadastro/terminal-cadastro.component';
import { TerminalListaComponent } from './terminal-lista/terminal-lista.component';
import { TerminalComponent } from './terminal.component';

const routes: Routes = [
  { 
    path: '', 
    component: TerminalComponent,
    children: [
      { 
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
      },
      { 
        path: 'lista', 
        component: TerminalListaComponent
      },
      {
        path: 'cadastro/add',
        component: TerminalCadastroComponent
      },
      {
        path: 'cadastro/edit/:id',
        component: TerminalCadastroComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TerminalRoutingModule { }
