import { Directive } from '@angular/core';
import { AbstractControl, ValidationErrors, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appOnlyAllspacecheck]',
  providers: [{ provide: NG_VALIDATORS, useExisting: OnlyAllSpaceDirective, multi: true }]
})
export class OnlyAllSpaceDirective {

  validate(control: AbstractControl): ValidationErrors {
    if (control.value === null || control.value === '' || control.value === undefined) {
      return null;
    }
    const regex = new RegExp('^\\s*$');
    const isValid = regex.test(control.value);

    return isValid ? { isallspace: true } : null;
  }
  constructor() { }
}
