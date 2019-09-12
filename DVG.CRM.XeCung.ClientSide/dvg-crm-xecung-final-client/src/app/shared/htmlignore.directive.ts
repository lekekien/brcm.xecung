import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appHtmlIgnore]',
  providers: [{ provide: NG_VALIDATORS, useExisting: HtmlIgnoreDirective, multi: true }]
})
export class HtmlIgnoreDirective implements Validator  {
  validate(control: AbstractControl): ValidationErrors {
    if (typeof control === undefined ||  control.value === null || control.value === '') {
      return null;
    }
    const regex: RegExp = /<[^>]+>/;
    const isValid = regex.test(control.value);
    return isValid ? { ishtml: true } : null;
  }
  constructor() { }
}
