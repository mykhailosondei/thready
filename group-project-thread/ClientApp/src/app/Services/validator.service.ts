import {Injectable} from '@angular/core';
import {HttpInternalService} from "./http-internal.service";
import {AbstractControl, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";

@Injectable({
  providedIn: 'root'
})
export class ValidatorService {

  constructor(private httpService: HttpInternalService) {
  }

  public isEmailAvailable(email: string) {
    return this.httpService.getFullRequest<boolean>(`/api/User/isEmailAvailable?email=${email}`);
  }

  public isValidEmail(email: string) {
    const atSymbolPosition = email.lastIndexOf('@');
    return !(atSymbolPosition < 0 || email.lastIndexOf('.') < atSymbolPosition || email.length - atSymbolPosition < 4);
  }

  public isUsernameAvailable(username: string) {
    return this.httpService.getFullRequest<boolean>(`/api/User/isUsernameAvailable?username=${username}`)
  }

  public isValidUserName(username: string) {
    return username != "" && username.length < 15;
  }

  passwordValidator(): Validators {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const password = control.value;

      if (password.length < 8) {
        return {'passwordLength': true};
      }

      return null;
    };
  }

  matchPassword: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    let password = control.get('password');
    let repeatedPassword = control.get('repeatedPassword');
    if (password && repeatedPassword && password?.value != repeatedPassword?.value) {
      return {passwordMatcherror: true}
    }
    return null;
  }

}
