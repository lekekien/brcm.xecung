import { Directive } from '@angular/core';
import { Validator, AbstractControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appPasswordValidator]',
  providers: [{ provide: NG_VALIDATORS, useExisting: PasswordValidatorDirective, multi: true }]
})
export class PasswordValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors {
    if (control.value === null || control.value === '' || control.value === undefined) {
      return null;
    }
    const checkSpace = new RegExp(/ /g);
    const isValid = checkSpace.test(control.value) || control.value.length < 6;
    return isValid ? { ispassword: true } : null;
  }
  constructor() { }
}
