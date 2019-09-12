import { Directive, Attribute } from '@angular/core';
import { Validator, AbstractControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appPasswordMatch]',
  providers: [{ provide: NG_VALIDATORS, useExisting: PasswordMatchDirective, multi: true }]
})
export class PasswordMatchDirective implements Validator {
  appPasswordMatchs: any;
  constructor(@Attribute('appPasswordMatch') public appPasswordMatch: string) {
    this.appPasswordMatchs = appPasswordMatch;
   }
  validate(control: AbstractControl): ValidationErrors {
    // self value (e.g. retype password)
    let value = control.value;
    // control value (e.g. password)
    let passwordField = control.root.get(this.appPasswordMatchs);
    // value not equal
    if (passwordField && value !== passwordField.value) {
      return {
      isPasswordMatch: true
    };
  }
    return null;
  }
}
