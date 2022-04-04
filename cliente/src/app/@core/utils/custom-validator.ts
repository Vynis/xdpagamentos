import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class CustomValidators {
    static dateMinimum(minDate: Date): ValidatorFn {
      return (control: AbstractControl): ValidationErrors | null => {
    // parse control value to Date
    const date = new Date(control.value);
    // check if control value is superior to date given in parameter
    console.log(minDate.getTime() );
    if (minDate.getTime() < date.getTime()) {
      return null;
    } else {
      return { 'min': { value: control.value, expected: minDate } };

    }
      };
    }
  }