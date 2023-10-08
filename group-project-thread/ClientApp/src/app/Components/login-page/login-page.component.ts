import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {finalize, Subject, takeUntil} from 'rxjs';
import { AuthService } from 'src/app/Services/auth.service';
import ValidateForm from 'src/app/helpers/validateForm';
import {SnackbarService} from "../../Services/snackbar.service";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss', '../../../assets/LoginAndRegisCommon.scss']
})
export class LoginPageComponent {
  passwordType: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash"
  loginForm!: FormGroup;
  private unsubscribe$ = new Subject<void>();
  private submitted: boolean = false;
  constructor(private fb: FormBuilder, private authService: AuthService,
              private router: Router, private snackbarService: SnackbarService) {

  }

  ngOnInit() : void{
    this.loginForm = this.fb.group({
      email: ['', Validators.required ],
      password: ['', Validators.required]
    })
  }
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.passwordType = "text" : this.passwordType = "password";
  }
  onSubmit(){

    if(this.loginForm.valid && !this.submitted){
      this.submitted = true;
      this.authService.login({email : this.loginForm.get('email')!.value,
        password: this.loginForm.get('password')!.value})
        .pipe(takeUntil(this.unsubscribe$), finalize(() => this.submitted = false))
      .subscribe(
        (response) => {
          this.router.navigate(['/mainPage']);
        },
        (error)=> {
          this.snackbarService.showErrorMessage(error.error.title)
        });
    }
    else{
      ValidateForm.validateAllFields(this.loginForm);
    }
  }
}

