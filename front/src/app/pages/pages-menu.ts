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
        link: '/pages/layout/list',
      },
      {
        title: 'Estabelecimentos',
        link: '/pages/layout/infinite-list',
      },
      {
        title: 'Usuários do Sistema',
        link: '/pages/usuario',
      },
      {
        title: 'Contas Caixa',
        pathMatch: 'prefix',
        link: '/pages/layout/tabs',
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
        title: 'Transferência de Transações para Outro Cliente',
        link: '/pages/ui-features/grid',
      },
      {
        title: 'Criação de Ordem de Pagamento',
        link: '/pages/ui-features/icons',
      },
      {
        title: 'Restauração de Ordem de Pagamento',
        link: '/pages/ui-features/typography',
      },
      {
        title: 'Gestão de Pagamento de Clientes',
        link: '/pages/ui-features/search-fields',
      },
      {
        title: 'Gestão de Extratos das Contas Caixa',
        link: '/pages/ui-features/search-fields',
      },
      {
        title: 'Resumo Gestão de Pagamento de Clientes',
        link: '/pages/ui-features/search-fields',
      },
      {
        title: 'Histórico de Importação de Arquivos',
        link: '/pages/ui-features/search-fields',
      },
    ],
  },
];
