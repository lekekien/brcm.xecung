import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appSmallerEqualZero]',
  providers: [{ provide: NG_VALIDATORS, useExisting: SmallerEqualZeroDirective, multi: true }]
})
export class SmallerEqualZeroDirective implements Validator  {
  validate(control: AbstractControl): ValidationErrors {
    if (typeof control === undefined ||  control.value === null || control.value === '') {
      return { isSmallerEqualZero: true };
    }
    const intParse = parseInt(control.value, 10);
    if (isNaN(intParse)) {
      return { isSmallerEqualZero: true };
    }
    if (intParse <= 0) {
      return { isSmallerEqualZero: true };
    } else {
      return null;
    }
  }
  constructor() { }
}
