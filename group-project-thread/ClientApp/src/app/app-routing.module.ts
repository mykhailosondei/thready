import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {LoginPageComponent} from './Components/login-page/login-page.component';
import {SignUpPageComponent} from './Components/sign-up-page/sign-up-page.component';
import {MainPageComponent} from "./Components/main-page/main-page.component";
import {ProfilePageComponent} from "./Components/profile-page/profile-page.component";

const routes: Routes = [
  {path:'', redirectTo: '/login', pathMatch: 'full'},
  {path:'login', component: LoginPageComponent},
  {path:'signup', component: SignUpPageComponent},
  {path:'mainPage', component: MainPageComponent},
  {path: 'profile', component: ProfilePageComponent}
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
