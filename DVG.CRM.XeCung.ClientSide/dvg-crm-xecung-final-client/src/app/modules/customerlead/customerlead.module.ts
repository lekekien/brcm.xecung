import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerleadRoutingModule } from './customerlead-routing.module';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';
import { CustomerleadComponent } from './components/customerlead/customerlead.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap';
import { CustomerleadconvertComponent } from './components/customerleadconvert/customerleadconvert.component';
import { LeadDataProviderService } from './services/lead-data-provider.service';
@NgModule({
  declarations: [
    CustomerleadComponent,
    CustomerleadconvertComponent
  ],
  imports: [
    CommonModule,
    CustomerleadRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedcomponentModule,
    DatepickerModule.forRoot(),
    BsDatepickerModule.forRoot()
  ],
  providers: [LeadDataProviderService]
})
export class CustomerleadModule { }
