import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerComponent } from './components/customer/customer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';
import { CustomerDataProviderService } from 'src/app/modules/customer/services/customer-data-provider.service';
import { CustomerDetailComponent } from './components/customer-detail/customer-detail.component';
import { CustomerUpdateComponent } from './components/customerupdate/customerupdate.component';
import { CustomerUpdateResolve } from 'src/app/resolve/customerupdate.resolve';

@NgModule({
  declarations: [
    CustomerComponent, 
    CustomerComponent,
    CustomerDetailComponent, 
    CustomerUpdateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CustomerRoutingModule,
    SharedcomponentModule
  ],
  providers: [
    CustomerDataProviderService,
    CustomerUpdateResolve
  ]
})
export class CustomerModule { }
