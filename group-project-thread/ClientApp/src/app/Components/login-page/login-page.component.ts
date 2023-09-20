import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/Services/auth.service';
import ValidateForm from 'src/app/helpers/validateForm';

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
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    
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
    if(this.loginForm.valid){
      this.authService.login({email : "test@gmail.com", password: "das"}).pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (response) => {
          console.log('Login successful:', response);
          this.router.navigate(['/signup']);
        },
        (error)=> {
          console.log("Login error", error);
        });
    }
    else{
      //throw the error
      ValidateForm.validateAllFields(this.loginForm);
    }
  }
}

