import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
//temp import { NotFoundComponent } from '../sharedcomponent/components/not-found/not-found.component';
import { ContactComponent } from './components/contact/contact.component';
import { ContactCreateComponent } from './components/contact-create/contact-create.component';

const routes: Routes = [
  { path: '', component: ContactComponent, canActivate: [AuthGuard]},
  { path: 'create', component: ContactCreateComponent, canActivate: [AuthGuard] },
  { path: 'edit/:id', component: ContactCreateComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactRoutingModule { }
