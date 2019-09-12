import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsersComponent } from './components/users/users.component';
import { CanActivate } from '@angular/router/src/utils/preactivation';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { UserupdateComponent } from './components/userupdate/userupdate.component';
import { UserprofileComponent } from './components/userprofile/userprofile.component';
import { OtpprivatekeyComponent } from './components/otpprivatekey/otpprivatekey.component';
import { Role } from 'src/app/enums/roles';

const routes: Routes = [
 {
  path: '',
  component: UsersComponent,
  canActivate: [AuthGuard],
  data: { roles: [Role.Admin] }
 },
 {
    path: 'update',
    component: UserupdateComponent,
    canActivate: [AuthGuard],
    data: { roles: [Role.Admin] }
  },
  {
    path: 'update/:id',
    component: UserupdateComponent,
    canActivate: [AuthGuard],
    data: { roles: [Role.Admin] }
  },
  {
    path: 'userprofile/:id',
    component: UserprofileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'otpprivatekey/:param',
    component: OtpprivatekeyComponent,
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
