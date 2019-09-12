import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerComponent } from './components/customer/customer.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { CustomerDetailComponent } from './components/customer-detail/customer-detail.component';
import { CustomerUpdateComponent } from './components/customerupdate/customerupdate.component';
import { CustomerUpdateResolve } from 'src/app/resolve/customerupdate.resolve';

const routes: Routes = [
  { path: '', component: CustomerComponent, canActivate: [AuthGuard]},
  { path: 'detail/:id', component: CustomerDetailComponent, canActivate: [AuthGuard]},
  { path: 'update',
    component: CustomerUpdateComponent,
    canActivate: [AuthGuard],
  },
  { path: 'update/:id',
    component: CustomerUpdateComponent,
    canActivate: [AuthGuard],
    resolve: { item: CustomerUpdateResolve }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
