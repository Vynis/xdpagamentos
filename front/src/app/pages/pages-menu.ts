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
        link: '/pages/cliente',
      },
      {
        title: 'POS/Terminais',
        link: '/pages/terminal',
      },
      {
        title: 'Estabelecimentos',
        link: '/pages/estabelecimento',
      },
      {
        title: 'Usuários do Sistema',
        link: '/pages/usuario',
      },
      {
        title: 'Contas Caixa',
        pathMatch: 'prefix',
        link: '/pages/conta',
      },
    ],
  },
  {
    title: 'Relatórios',
    icon: 'edit-2-outline',
    children: [
      {
        title: 'Solicitações',
        link: '/pages/relatorio/solicitacoes',
      },
      {
        title: 'Saldo de Clientes',
        link: '/pages/forms/layouts',
      },
      {
        title: 'Saldo das Contas Correntes',
        link: '/pages/forms/buttons',
      }
    ],
  },
  {
    title: 'Ajustes',
    icon: 'keypad-outline',
    children: [
      {
        title: 'Transferência de Transações',
        link: '/pages/transferencia-transacao',
      },
      {
        title: 'Gestão de Pagto de Clientes',
        link: '/pages/gestao-pagto',
      },
      {
        title: 'Gestão de Extratos',
        link: '/pages/gestao-extrato',
      }
    ],
  },
];
