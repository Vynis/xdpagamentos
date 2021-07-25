import { Router } from '@angular/router';
/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Component, OnInit } from '@angular/core';
import { NbMenuService } from '@nebular/theme';
import { AnalyticsService } from './@core/utils/analytics.service';
import { SeoService } from './@core/utils/seo.service';

@Component({
  selector: 'ngx-app',
  template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit {

  constructor(
    private analytics: AnalyticsService, 
    private seoService: SeoService, 
    private menuService: NbMenuService,
    private router: Router
    ) {
      this.menuUser();
  }

  ngOnInit(): void {
    this.analytics.trackPageViews();
    this.seoService.trackCanonicalChanges();
  }

  menuUser() {
    this.menuService.onItemClick().subscribe(
      event => {

        switch (event.item.title ) {
          case 'Sair':
            localStorage.clear();
            this.router.navigateByUrl('/auth/login');
            break;
          case 'Alterar Senha':
            this.router.navigateByUrl('/pages/dashboard');
            break;
          default:
            break;
        }
      }
    )
  }
}
