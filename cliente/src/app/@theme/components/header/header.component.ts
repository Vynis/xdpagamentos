import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { NbDialogService, NbMediaBreakpointsService, NbMenuService, NbSidebarService, NbThemeService } from '@nebular/theme';

import { UserData } from '../../../@core/data/users';
import { LayoutService } from '../../../@core/utils';
import { map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { GestaoPagamentoData } from '../../../@core/data/gestao-pagto';

import { ClienteData } from '../../../@core/data/cliente';
import { ClienteModel } from '../../../@core/models/cliente.model';
import { GeralEnum } from '../../../@core/enums/geral.enum';


@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit, OnDestroy {

  private destroy$: Subject<void> = new Subject<void>();
  userPictureOnly: boolean = false;
  user: any;
  saldo: string = '0,00';
  limite: string = '0,00';
  total: string = '0,00';
  listaClientes: ClienteModel[];
  selectValue;
  cliIdOld=0;

  themes = [
    {
      value: 'default',
      name: 'Light',
    },
    {
      value: 'dark',
      name: 'Dark',
    },
    {
      value: 'cosmic',
      name: 'Cosmic',
    },
    {
      value: 'corporate',
      name: 'Corporate',
    },
  ];

  currentTheme = 'default';

hideMenuOnClick: boolean = false;

  userMenu = [ { title: 'Alterar Senha' }, { title: 'Sair' } ];

  constructor(private sidebarService: NbSidebarService,
              private menuService: NbMenuService,
              private themeService: NbThemeService,
              private userService: UserData,
              private layoutService: LayoutService,
              private breakpointService: NbMediaBreakpointsService,
              private gestaoPagamentoService: GestaoPagamentoData,
              private clienteService: ClienteData,
              private dialogService: NbDialogService) {
  }


  ngOnInit() {
    this.currentTheme = this.themeService.currentTheme;

    //this.buscarSaldo();
    this.cliIdOld = Number(localStorage.getItem(GeralEnum.IDCLIENTE));
    this.buscarCliente();

    this.userService.getUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe((users: any) => this.user = users);

    const {xl} = this.breakpointService.getBreakpointsMap();
    const {is} = this.breakpointService.getBreakpointsMap();
    this.themeService.onMediaQueryChange()
      .pipe(
        map(([, currentBreakpoint]) => currentBreakpoint),
        takeUntil(this.destroy$),
      )
      .subscribe(currentBreakpoint => {
        this.userPictureOnly = currentBreakpoint.width < xl;
        this.hideMenuOnClick = currentBreakpoint.width <= is;
      });

      this.menuService.onItemClick().subscribe(() => {
        if (this.hideMenuOnClick) {
          this.sidebarService.collapse('menu-sidebar');
        }
      });

    this.themeService.onThemeChange()
      .pipe(
        map(({ name }) => name),
        takeUntil(this.destroy$),
      )
      .subscribe(themeName => this.currentTheme = themeName);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  changeTheme(themeName: string) {
    this.themeService.changeTheme(themeName);
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  navigateHome() {
    this.menuService.navigateHome();
    return false;
  }

  buscarSaldo(id: number) {
    this.gestaoPagamentoService.saldoAtual(id).subscribe(
      res => {
        if (!res.success)
          return;
        
        this.saldo = res.data.saldoCliente;
        this.limite = res.data.limiteCliente;
        this.total = res.data.total;
      }
    )
  }

  buscarCliente() {
    this.clienteService.buscarAtivos().subscribe(
      res => {
        if (!res.success)
          return;

        this.listaClientes = res.data;

        if (this.listaClientes.length > 0){
          if (this.cliIdOld !== 0) {
            this.selectValue = this.cliIdOld;
            this.buscarSaldo(this.cliIdOld);
            return;
          }

          this.selectValue = this.listaClientes[0].id;
          this.buscarSaldo(this.listaClientes[0].id);
          localStorage.setItem(GeralEnum.IDCLIENTE,this.selectValue );
        }
         
      }
    )
  }

  open(dialog: TemplateRef<any>) {
    this.dialogService.open(dialog);
  }

  changeCliente(valor) {
    localStorage.setItem(GeralEnum.IDCLIENTE,valor);
    location.reload();
  }
}
