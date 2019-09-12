import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdvertisementsComponent } from './components/advertisements/advertisements.component';
import{ AdvertisementCreateComponent } from './components/advertisement-create/advertisement-create.component'
import { AdvertisementRoutingModule } from './advertisement-routing.module'
import { FormsModule } from '@angular/forms';
import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';
@NgModule({
  declarations: [AdvertisementsComponent, AdvertisementCreateComponent],
  imports: [
    FormsModule,
    CommonModule,
    AdvertisementRoutingModule,
    SharedcomponentModule,
    DatepickerModule.forRoot(),
    BsDatepickerModule.forRoot()
  ]
})
export class AdvertisementModule { }