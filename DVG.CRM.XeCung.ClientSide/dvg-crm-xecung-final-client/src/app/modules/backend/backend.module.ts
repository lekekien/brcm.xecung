import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BackendRoutingModule } from './backend-routing.module';
import { BackendComponent } from './components/backend/backend.component';
import { SharedcomponentModule } from '../sharedcomponent/sharedcomponent.module';

@NgModule({
  declarations: [BackendComponent],
  imports: [
    CommonModule,
    BackendRoutingModule,
    SharedcomponentModule
  ]
})
export class BackendModule { }
