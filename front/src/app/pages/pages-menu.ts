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
        title: 'Pre-Venda',
        link: '/pages/forms/inputs',
      },
      {
        title: 'Pagamentos/Reembolso à Clientes',
        link: '/pages/forms/layouts',
      },
      {
        title: 'Pagamentos Analítico por Cliente e Período',
        link: '/pages/forms/buttons',
      },
      {
        title: 'Vendas',
        link: '/pages/forms/datepicker',
      },
      {
        title: 'Terminais',
        link: '/pages/forms/datepicker',
      },
      {
        title: 'Transações com a Operadora',
        link: '/pages/forms/datepicker',
      },
      {
        title: 'Extrato de Contas Caixa',
        link: '/pages/forms/datepicker',
      },
      {
        title: 'Extrato de Contas Caixa Detalhado',
        link: '/pages/forms/datepicker',
      },
    ],
  },
  {
    title: 'Ajustes',
    icon: 'keypad-outline',
    children: [
      {
        title: 'Transferência de Transações',
        link: '/pages/ui-features/grid',
      },
      {
        title: 'Criação de Ordem de Pagto',
        link: '/pages/geracao-pagto',
      },
      {
        title: 'Restauração de Ordem de Pagto',
        link: '/pages/ui-features/typography',
      },
      {
        title: 'Gestão de Pagto de Clientes',
        link: '/pages/gestao-pagto',
      },
      {
        title: 'Gestão de Extratos',
        link: '/pages/gestao-extrato',
      },
      {
        title: 'Resumo Gestão de Pagto',
        link: '/pages/ui-features/search-fields',
      }
    ],
  },
];
