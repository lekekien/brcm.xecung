import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { AdvertisementsComponent } from './components/advertisements/advertisements.component';
import { AdvertisementCreateComponent } from './components/advertisement-create/advertisement-create.component';
// import { advertisementEditComponent } from './components/advertisement-edit/advertisement-edit.component';
// import { advertisementDetailComponent } from './components/advertisement-detail/advertisement-detail.component';

const routes: Routes = [
  {
   path: '',
   component: AdvertisementsComponent,
   canActivate: [AuthGuard],
   //data: { roles: [Role.Admin] }
  },
  {
    path: 'create',
    component: AdvertisementCreateComponent,
    canActivate: [AuthGuard],
  },
 
 ];
 
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdvertisementRoutingModule { }
