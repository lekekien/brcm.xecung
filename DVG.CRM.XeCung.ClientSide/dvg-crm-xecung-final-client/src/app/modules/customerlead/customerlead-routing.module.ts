import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerleadComponent } from './components/customerlead/customerlead.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { CustomerleadconvertComponent } from './components/customerleadconvert/customerleadconvert.component';
//temp import { NotFoundComponent } from '../sharedcomponent/components/not-found/not-found.component';

const routes: Routes = [
  { path: '', component: CustomerleadComponent, canActivate: [AuthGuard] },
  { path: 'convert', component: CustomerleadconvertComponent, canActivate: [AuthGuard]},
  //temp { path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerleadRoutingModule { }
