import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'Dashboard',
    icon: 'shopping-cart-outline',
    link: '/pages/dashboard',
    home: true,
  },
  {
    title: 'Administrativo',
    icon: 'layout-outline',
    children: [
      {
        title: 'Clientes',
        pathMatch: 'prefix',
        link: '/pages/cliente',
      },
      {
        title: 'Estabelecimentos',
        pathMatch: 'prefix',
        link: '/pages/estabelecimento',
      },
      {
        title: 'POS/Terminais',
        pathMatch: 'prefix',
        link: '/pages/terminal',
      },
      {
        title: 'Usuários do Sistema',
        pathMatch: 'prefix',
        link: '/pages/usuario',
      },
      {
        title: 'Usuários de Clientes',
        pathMatch: 'prefix',
        link: '/pages/usuario-cliente',
      }

    ],
  },
  {
    title: 'Relatórios',
    icon: 'edit-2-outline',
    children: [
      {
        title: 'Solicitações',
        pathMatch: 'prefix',
        link: '/pages/relatorio/solicitacoes',
      },
      {
        title: 'Saldo de Clientes',
        pathMatch: 'prefix',
        link: '/pages/relatorio/saldo-clientes',
      },
      {
        title: 'Saldo das Contas Correntes',
        pathMatch: 'prefix',
        link: '/pages/relatorio/saldo-conta-correntes',
      },
      {
        title: 'Geral de Vendas',
        pathMatch: 'prefix',
        link: '/pages/relatorio/geral-vendas'
      }
    ],
  },
  {
    title: 'Ajustes',
    icon: 'keypad-outline',
    children: [
      {
        title: 'Transferência de Transações',
        pathMatch: 'prefix',
        link: '/pages/transferencia-transacao',
      },
      {
        title: 'Gestão de Pagto de Clientes',
        pathMatch: 'prefix',
        link: '/pages/gestao-pagto',
      },
      {
        title: 'Gestão de Extratos',
        pathMatch: 'prefix',
        link: '/pages/gestao-extrato',
      }
    ],
  },
  {
    title: 'Financeiro',
    icon: 'calendar-outline',
    children: [
      {
        title: 'Contas Caixa',
        pathMatch: 'prefix',
        link: '/pages/conta',
      },
      {
        title: 'Centro Custo',
        pathMatch: 'prefix',
        link: '/pages/centro-custo'
      },
      {
        title: 'Conta Pagar',
        pathMatch: 'prefix',
        link: '/pages/conta-pagar'
      },
      {
        title: 'Conta Receber',
        pathMatch: 'prefix',
        link: '/pages/conta-receber'
      },
    ]
  }
];
