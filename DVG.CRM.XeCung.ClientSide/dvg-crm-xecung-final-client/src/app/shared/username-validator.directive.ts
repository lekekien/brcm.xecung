import { Directive } from '@angular/core';
import { Validator, AbstractControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appUsernameValidator]',
  providers: [{ provide: NG_VALIDATORS, useExisting: UsernameValidatorDirective, multi: true }]
})
export class UsernameValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors {
    if (control.value === null || control.value === '' || control.value === undefined) {
      return null;
    }
    const regex = new RegExp('^[a-z0-9A-Z]+$');
    // const checkSpace = new RegExp(/ /g);
    const isValid = regex.test(control.value) && control.value.length < 51;
    return !isValid ? { isusername: true } : null;
  }
  constructor() { }
}
