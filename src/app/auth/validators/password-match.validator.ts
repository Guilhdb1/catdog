import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordMatchValidator(passwordField: string, confirmField: string): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const password = group.get(passwordField);
    const confirm = group.get(confirmField);

    if (!password || !confirm) {
      return null;
    }

    if (confirm.errors && !confirm.errors['passwordMismatch']) {
      return null;
    }

    if (password.value !== confirm.value) {
      confirm.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }

    confirm.setErrors(null);
    return null;
  };
}
