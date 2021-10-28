import { Component } from '@angular/core';
import { SessoesEnum } from '../../@core/enums/sessoes.enum';
import { AuthServiceService } from '../../@core/services/auth-service.service';

@Component({
  selector: 'ngx-ecommerce',
  templateUrl: './e-commerce.component.html',
})
export class ECommerceComponent {


  constructor(private authService: AuthServiceService) {
    this.authService.validaPermissaoTela(SessoesEnum.DASHBOARD);

  }
}
