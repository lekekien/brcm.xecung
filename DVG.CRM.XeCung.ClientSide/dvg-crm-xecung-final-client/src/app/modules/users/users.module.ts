import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';
import { UsersComponent } from './components/users/users.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { UserupdateComponent } from './components/userupdate/userupdate.component';
import { UserprofileComponent } from './components/userprofile/userprofile.component';
import { OtpprivatekeyComponent } from './components/otpprivatekey/otpprivatekey.component';
import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [
    UsersComponent,
    UserupdateComponent,
    UserprofileComponent,
    OtpprivatekeyComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    UserRoutingModule,
    SharedcomponentModule,
    DatepickerModule.forRoot(),
    BsDatepickerModule.forRoot()
  ]
})
export class UsersModule { }
