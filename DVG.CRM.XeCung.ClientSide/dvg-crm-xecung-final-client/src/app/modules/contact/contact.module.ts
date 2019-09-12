import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContactComponent } from './components/contact/contact.component';
import { ContactRoutingModule } from './contact-routing.module';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ContactCreateComponent } from './components/contact-create/contact-create.component';
//temp import { ContactCreateComponent } from './components/contact-create/contact-create.component';

@NgModule({
  declarations: [ContactComponent, ContactCreateComponent,
     //temp ContactCreateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ContactRoutingModule,
    SharedcomponentModule
  ]
})
export class ContactModule { }
