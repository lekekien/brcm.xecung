import { Directive } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, Validator, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appPhonenumberValidator]',
  providers: [{ provide: NG_VALIDATORS, useExisting: PhonenumberValidatorDirective, multi: true }]
})
export class PhonenumberValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors {
    if (control.value === null || control.value === '' || control.value === undefined) {
      return null;
    }
    const regex: RegExp = /^0-?\d{1,3}-?\d{3,4}-?\d{4}$/;
    const isValid = regex.test(control.value);
    return !isValid ? { isphone: true } : null;
  }
  constructor() { }
}
