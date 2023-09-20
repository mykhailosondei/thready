import { HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/Services/auth.service';
import ValidateForm from 'src/app/helpers/validateForm';
import { RegisterUserDTO } from 'src/app/models/auth/registerUserDTO';

@Component({
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html',
  styleUrls: ['./sign-up-page.component.scss', '../../../assets/LoginAndRegisCommon.scss'],
})
export class SignUpPageComponent {
  passwordType: string = "password";
  repeatedPasswordType: string = "password";
  isText: boolean = false;
  isTextRepeatedPass: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  repeatedEyeIcon: string = "fa-eye-slash";
  originalPassword: string = "";
  repeatedPassword: string = "";
  isEqualMessage: string = "";
  isEnoughChrMessage: string = "";
  messageColor: string = "red";
  passwordsMatch: boolean = false;
  selectedDay: number | null = null;
  days: number[] = [];
  selectedYear: number | null = null;
  years: number[] = [];
  selectedMonth: number | null = null;
  months: { value: number; name: string }[] = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];
  regisForm!: FormGroup;
  private unsubscribe$ = new Subject<void>();
  

  constructor(private fb: FormBuilder, private authService : AuthService, private router: Router) {
    for (let i = 1; i <= 31; i++) {
      this.days.push(i);
    }
    for (let i = 2023; i>=1904;i--){
      this.years.push(i);
    }
  }

  ngOnInit() : void{
    this.regisForm=this.fb.group({
      username: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', [Validators.required, this.passwordValidator()]],
      repeatedPassword: ['', [Validators.required]],
      selectedMonth: ['', Validators.required],
      selectedDay: ['', Validators.required],
      selectedYear: ['', Validators.required]
    },
    {
      validators : this.matchPassword
    })
  }

  matchPassword : ValidatorFn = (control : AbstractControl) : ValidationErrors | null => {
    let password  = control.get('password');
    let repeatedPassword = control.get('repeatedPassword');
    if (password && repeatedPassword && password?.value != repeatedPassword?.value){
      return {passwordMatcherror : true}
    }
    return null;
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  createUserFromForm(): RegisterUserDTO{
      const registerUser: RegisterUserDTO = {
      username: this.regisForm.get('username')!.value,
      email: this.regisForm.get('email')!.value,
      password: this.regisForm.get('password')!.value,
      dateOfBirth: new Date(
        this.regisForm.get('selectedYear')!.value,
        this.regisForm.get('selectedMonth')!.value - 1, 
        this.regisForm.get('selectedDay')!.value
      ),
    };
    return registerUser;

  }

  onSubmit(){
    if(this.regisForm.valid){
      this.authService.register({username : "John", email : "test@gmail.com", password: "das", dateOfBirth: new Date(2000, 11, 30) }).pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (response) => {
          console.log('Registration successful:', response);
          this.router.navigate(['/login']);
        },
        (error)=> {
          console.log(this.createUserFromForm());
          console.log("registration error", error);
        });
    }
    else{
      console.log(this.findInvalidControls());
      ValidateForm.validateAllFields(this.regisForm);
    }
  }

  public findInvalidControls() {
    const invalid = [];
    const controls = this.regisForm.controls;
    for (const name in controls) {
        if (controls[name].invalid) {
            invalid.push(name);
        }
    }
    return invalid;
}
  
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.passwordType = "text" : this.passwordType = "password";
  }

  hideShowRepeatPass(){
    this.isTextRepeatedPass = !this.isTextRepeatedPass;
    this.isTextRepeatedPass ? this.repeatedEyeIcon = "fa-eye" : this.repeatedEyeIcon = "fa-eye-slash";
    this.isTextRepeatedPass ? this.repeatedPasswordType = "text" : this.repeatedPasswordType = "password";
  }

  passwordValidator(): Validators {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const password = control.value;

      
      if (password.length < 8) {
        return { 'passwordLength': true };
      }

      return null; 
    };
  }

  updateDates(){
    let daysInMonth: number = this.daysBasedOnDropDowns();
    
    this.days = [];
    for (let i = 1; i <= daysInMonth; i++) {
    this.days.push(i);
    }
  }
  
  daysBasedOnDropDowns(): number{
    if (this.selectedYear != null && this.selectedMonth != null) {
      return new Date(this.selectedYear, this.selectedMonth, 0).getDate();
    } 
    else if(this.selectedMonth != null){ 
      return this.getDaysInMonth();
    }
    else {
      return 31;
    }
  }
  
  getDaysInMonth(): number{
    
    let thirtyDays: number[] = [4, 6, 9, 11];
    let monthToNum: number = Number(this.selectedMonth!);
    console.log(typeof(monthToNum));
    if(thirtyDays.includes(monthToNum)){
      return 30;
    }
    else if (monthToNum === 2){
      return 29;
    }
    else{
      return 31;
    }
  }

  
}
