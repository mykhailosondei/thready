import {HttpHeaders, HttpResponse} from '@angular/common/http';
import {Component, Inject} from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {finalize, map, Subject, takeUntil} from 'rxjs';
import { AuthService } from 'src/app/Services/auth.service';
import ValidateForm from 'src/app/helpers/validateForm';
import { RegisterUserDTO } from 'src/app/models/auth/registerUserDTO';
import { formatDate} from "@angular/common";
import {SnackbarService} from "../../Services/snackbar.service";
import {ValidatorService} from "../../Services/validator.service";

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
  private submitted: boolean = false;
  private timeout: any = null;
  emailAvailabilityMessage : string = "";
  usernameAvailabilityMessage : string = "";

  constructor(private fb: FormBuilder, private authService : AuthService,
              private router: Router, private snackBarService : SnackbarService,
              private availabilityService : ValidatorService) {
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
      password: ['', [Validators.required, this.availabilityService.passwordValidator()]],
      repeatedPassword: ['', [Validators.required]],
      selectedMonth: ['', Validators.required],
      selectedDay: ['', Validators.required],
      selectedYear: ['', Validators.required]
    },
    {
      validators : this.availabilityService.matchPassword
    })
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
      dateOfBirth: formatDate( new Date(
        this.regisForm.get('selectedYear')!.value,
        this.regisForm.get('selectedMonth')!.value - 1,
        this.regisForm.get('selectedDay')!.value
      ), "yyyy-MM-dd", "en-US", "UTC")
    };
    return registerUser;

  }

  onSubmit(){
    if(this.regisForm.valid && !this.submitted && this.emailAvailabilityMessage == "" && this.usernameAvailabilityMessage == ""){
      this.submitted = true;
      this.authService.register( this.createUserFromForm())
        .pipe(takeUntil(this.unsubscribe$), finalize(() => this.submitted = false))
      .subscribe(
        () => {
          this.router.navigate(['/login']);
        },
        (error)=> {
          this.snackBarService.showErrorMessage(error.error.title);
        });
    }
    else{
      ValidateForm.validateAllFields(this.regisForm);
    }
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


   onEmailInput(event: KeyboardEvent) {
    const target = event.target as HTMLInputElement;
    if (!this.availabilityService.isValidEmail(target.value)){
      this.emailAvailabilityMessage = "Provide valid email";
      return;
    }
    this.emailAvailabilityMessage = "";
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {

      if (event.keyCode !== 13) {
        if (target) {
          this.availabilityService.isEmailAvailable(target.value)
            .pipe(
              takeUntil(this.unsubscribe$)).subscribe(
            (responce : HttpResponse<boolean>) => {
              const result = responce.body;
              if (result){
                this.emailAvailabilityMessage = "";
              }
              else {
                this.emailAvailabilityMessage = "Email already in use"
              }
            })
        }
      }
    }, 1000);
  }

  onUsernameInput(event: KeyboardEvent) {
    const target = event.target as HTMLInputElement;
    if (!this.availabilityService.isValidUserName(target.value)){
      this.usernameAvailabilityMessage = "Provide valid username";
      return;
    }
    this.usernameAvailabilityMessage = "";
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {

      if (event.keyCode !== 13) {
        if (target) {
          this.availabilityService.isUsernameAvailable(target.value)
            .pipe(
              takeUntil(this.unsubscribe$)).subscribe(
            (responce : HttpResponse<boolean>) => {
              const result = responce.body;
              if (result){
                this.usernameAvailabilityMessage = "";
              }
              else {
                this.usernameAvailabilityMessage = "There is user with such username"
              }
            })
        }
      }
    }, 1000);
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
      console.log(this.selectedMonth);
      return 31;
    }
  }

  getDaysInMonth(): number{

    let thirtyDays: number[] = [4, 6, 9, 11];
    let monthToNum: number = Number(this.selectedMonth!);
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
