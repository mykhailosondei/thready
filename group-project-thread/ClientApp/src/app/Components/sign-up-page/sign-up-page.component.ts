import { Component } from '@angular/core';

@Component({
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html',
  styleUrls: ['./sign-up-page.component.scss', '../../../assets/LoginAndRegisCommon.scss']
})
export class SignUpPageComponent {
  passType: string = "password";
  repeatPassType: string = "password";
  isText: boolean = false;
  isTextRepeatPass: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  repeatEyeIcon: string = "fa-eye-slash";
  firstpassword: string = "";
  repeatedPassword: string = "";
  isEqualMessage: string = "";
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

  constructor() {
    // Generate an array of days (1 to 31) and populate the 'days' property
    for (let i = 1; i <= 31; i++) {
      this.days.push(i);
    }
    for (let i = 2023; i>=1904;i--){
      this.years.push(i);
    }
  }
  ngOnInit() : void{

  }
  
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.passType = "text" : this.passType = "password";
  }

  hideShowRepeatPass(){
    this.isTextRepeatPass = !this.isTextRepeatPass;
    this.isTextRepeatPass ? this.repeatEyeIcon = "fa-eye" : this.repeatEyeIcon = "fa-eye-slash";
    this.isTextRepeatPass ? this.repeatPassType = "text" : this.repeatPassType = "password";
  }

  checkPasswordMatch() {
    if (this.firstpassword === this.repeatedPassword) {
      this.isEqualMessage = "";
    } else {
      this.messageColor = "red";
      this.isEqualMessage = "Passwords are different";
    }
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
