import { Routes } from '@angular/router';
import { Pages } from './pages/pages';
import { Dashboard } from './pages/dashboard/dashboard';
import { CustomerDetail } from './pages/customer-detail/customer-detail';

export const routes: Routes = [
  { path: '', redirectTo: 'customer', pathMatch: 'full' },
  {
    path: '',
    component: Pages,
    children: [
      { path: 'customer', component: Dashboard },
      { path: 'create-customer', component: CustomerDetail },
      { path: 'update-customer/:id', component: CustomerDetail },

    ],
  },
];
