import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './modules/main/main.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedcomponentModule } from './modules/sharedcomponent/sharedcomponent.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { ScriptLoaderService } from './services/script-loader.service';
import { DataSharingService } from './services/data-sharing.service';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent
  ],
  imports: [
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    SharedcomponentModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [ScriptLoaderService, CookieService, DataSharingService],
  bootstrap: [AppComponent]
})
export class AppModule { }
