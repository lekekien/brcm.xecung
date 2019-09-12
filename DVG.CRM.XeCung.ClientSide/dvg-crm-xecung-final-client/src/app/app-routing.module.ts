import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './modules/main/main.component';
import { Role } from './enums/roles';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: '', component: MainComponent,
    children: [

      { path: 'home', loadChildren: './modules/backend/backend.module#BackendModule' },
      { path: 'customer', loadChildren: './modules/customer/customer.module#CustomerModule' },
      { path: 'videos', loadChildren: './modules/video/video.module#VideoModule' },
      { path: 'advertisements', loadChildren: './modules/advertisement/advertisement.module#AdvertisementModule'},
      // { path: 'payment', loadChildren: './modules/payment/payment.module#PaymentModule' },
      {
        path: 'users', loadChildren: './modules/users/users.module#UsersModule',
      },
      { path: 'report', loadChildren: './modules/report/report.module#ReportModule', data: { roles: [Role.Admin, Role.Manager] } },
      { path: 'contact', loadChildren: './modules/contact/contact.module#ContactModule' },
      { path: 'customerlead', loadChildren: './modules/customerlead/customerlead.module#CustomerleadModule' }
    ]
  },
  { path: 'login', loadChildren: './modules/login/login.module#LoginModule' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
