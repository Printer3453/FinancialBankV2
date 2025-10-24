import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {

        path: '/',
        name: 'Hesaplarım',
        iconClass: 'fas fa-wallet',
        order: 1,
        layout: eLayoutType.application,

      },
      {
        path: '/money-transfer',
        name: 'Para Transferi',
        iconClass: 'fas fa-exchange-alt',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/history',
        name: 'İşlem Geçmişi',
        iconClass: 'fas fa-history',
        order: 3,
        layout: eLayoutType.application,
      },
    ]);
  };
}
