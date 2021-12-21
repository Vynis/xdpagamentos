import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'Dashboard',
    icon: 'shopping-cart-outline',
    link: '/pages/dashboard',
    home: true,
  },
  {
    title: 'Dados',
    icon: 'layout-outline',
    children: [
      {
        title: 'Minha conta',
        link: '/pages/cliente',
      },
      {
        title: 'Extrato',
        link: '/pages/terminal',
      },
    ],
  }
];
