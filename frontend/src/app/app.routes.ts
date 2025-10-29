import { Routes } from '@angular/router';
import { Pages } from './pages/pages';
import { Dashboard } from './pages/dashboard/dashboard';
import { TableWork } from './pages/table-work/table-work';
import { All } from './pages/all/all';
import { Merchandise } from './pages/merchandise/merchandise';
import { Opportunity } from './pages/opportunity/opportunity';
import { Orders } from './pages/orders/orders';
import { Quote } from './pages/quote/quote';
import { Report } from './pages/report/report';
import { CustomerDetail } from './pages/customer-detail/customer-detail';

export const routes: Routes = [
  { path: '', redirectTo: 'customer', pathMatch: 'full' },
  {
    path: '',
    component: Pages,
    children: [
      { path: 'customer', component: Dashboard },
      { path: 'table-work', component: TableWork },
      { path: 'all', component: All },
      { path: 'merchandise', component: Merchandise },
      { path: 'opportunity', component: Opportunity },
      { path: 'orders', component: Orders },
      { path: 'quote', component: Quote },
      { path: 'report', component: Report },
      { path: 'create-customer', component: CustomerDetail },

      { path: 'update-customer/:id', component: CustomerDetail },

    ],
  },
];
