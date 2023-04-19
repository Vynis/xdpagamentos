import { CommonModule } from "@angular/common";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { NbCardModule, NbInputModule, NbButtonModule, NbSelectModule, NbTabsetModule, NbAlertModule, NbTooltipModule } from "@nebular/theme";
import { Ng2SmartTableModule } from "ng2-smart-table";
import { InterceptService } from "../../@core/utils/intercept.service";
import { UsuarioClienteCadastroComponent } from "./usuario-cliente-cadastro/usuario-cliente-cadastro.component";
import { UsuarioClienteListaComponent } from "./usuario-cliente-lista/usuario-cliente-lista.component";
import { UsuarioClienteRoutingModule } from "./usuario-cliente-routing.module";
import { UsuarioClienteComponent } from "./usuario-cliente.component";
import { UsuarioClienteService } from "../../@core/services/usuario-cliente.service";


@NgModule({
  declarations: [
    UsuarioClienteComponent,
    UsuarioClienteListaComponent,
    UsuarioClienteCadastroComponent
  ],
  imports: [
    CommonModule,
    UsuarioClienteRoutingModule,
    NbCardModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbTabsetModule,
    ReactiveFormsModule,
    NbAlertModule,
    Ng2SmartTableModule,
    NbTooltipModule
    
  ],
  providers: [
    InterceptService,
    {
      provide: HTTP_INTERCEPTORS,
        useClass: InterceptService,
      multi: true
    },
    UsuarioClienteService
  ]
})
export class UsuarioClienteModule { }
