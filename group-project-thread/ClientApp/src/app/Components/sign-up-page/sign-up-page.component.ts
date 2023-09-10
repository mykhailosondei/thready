import { Component } from '@angular/core';

@Component({
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html',
  styleUrls: ['./sign-up-page.component.scss']
})
export class SignUpPageComponent {
  passType: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash"
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

  updateDates(){
    let daysInMonth: number = 0;
    if (this.selectedYear !== null && this.selectedMonth !== null) {
      daysInMonth = new Date(this.selectedYear, this.selectedMonth, 0).getDate();
      // Now you can safely use daysInMonth
    } 
    else if(this.selectedYear !== null || this.selectedMonth !== null){
      daysInMonth = 31;
    }
    else {
      // Handle the case where either selectedYear or selectedMonth is null
      console.error('selectedYear or selectedMonth is null.');
    }
    this.days = [];
    for (let i = 1; i <= daysInMonth; i++) {
    this.days.push(i);
}
  }
}
