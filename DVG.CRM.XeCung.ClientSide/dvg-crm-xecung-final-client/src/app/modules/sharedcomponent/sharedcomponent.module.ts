import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuBarComponent } from './components/menu-bar/menu-bar.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { BaseComponent } from './components/base/base.component';
import { PhonenumberValidatorDirective } from 'src/app/shared/phonenumber-validator.directive';
import { HtmlIgnoreDirective } from 'src/app/shared/htmlignore.directive';
import { RouterModule } from '@angular/router';
import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap';
import { OnlyAllSpaceDirective } from 'src/app/shared/onlyallspace-validator.directive';
import { PasswordValidatorDirective } from 'src/app/shared/password-validator.directive';
import { UsernameValidatorDirective } from 'src/app/shared/username-validator.directive';
import { PagingComponent } from './components/paging/paging.component';
import { DatepickercustomComponent } from './components/datepickercustom/datepickercustom.component';
import { SmallerEqualZeroDirective } from 'src/app/shared/smallerequalzero.directive';
import { PasswordMatchDirective } from 'src/app/shared/password-match.directive';

@NgModule({
  declarations: [
    BaseComponent,
    FooterComponent,
    HeaderComponent,
    MenuBarComponent,
    PhonenumberValidatorDirective,
    HtmlIgnoreDirective,
    OnlyAllSpaceDirective,
    PasswordValidatorDirective,
    UsernameValidatorDirective,
    PagingComponent,
    DatepickercustomComponent,
    SmallerEqualZeroDirective,
    PasswordMatchDirective
  ],
  imports: [
    RouterModule,
    CommonModule,
    DatepickerModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ],
  exports: [
    BaseComponent,
    FooterComponent,
    HeaderComponent,
    MenuBarComponent,
    PhonenumberValidatorDirective,
    HtmlIgnoreDirective,
    OnlyAllSpaceDirective,
    PasswordValidatorDirective,
    UsernameValidatorDirective,
    PagingComponent,
    DatepickercustomComponent,
    SmallerEqualZeroDirective,
    PasswordMatchDirective
  ]
})
export class SharedcomponentModule { }
