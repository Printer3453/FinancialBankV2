import { Routes } from '@angular/router';
import { AccountsDashboardComponent } from './pages/accounts-dashboard/accounts-dashboard.component';
import { MoneyTransferComponent } from './pages/money-transfer/money-transfer.component'; 
import { ReceiptComponent } from './pages/receipt/receipt.component';
import { TransactionHistoryComponent } from './pages/transaction-history/transaction-history.component';

export const appRoutes: Routes = [
  {
    path: '',
    component: AccountsDashboardComponent,
    pathMatch: 'full',
  },
  { 
    path: 'receipt/:id',
    component: ReceiptComponent,
  },
   { 
    path: 'history',
    component: TransactionHistoryComponent,
  },
  { 
    path: 'money-transfer',
    component: MoneyTransferComponent,
  },
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.routes').then(m => m.homeRoutes),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.createRoutes()),
  },
];
