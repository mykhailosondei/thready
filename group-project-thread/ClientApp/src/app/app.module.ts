import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {LoginPageComponent} from './Components/login-page/login-page.component';
import {SignUpPageComponent} from './Components/sign-up-page/sign-up-page.component';
import {AppRoutingModule} from './app-routing.module';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {NotifierComponent} from "./Components/notifier/notifier.component";
import {MainPageComponent} from "./Components/main-page/main-page.component";
import {PagePostComponent} from "./Components/page-post/page-post.component";
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {ProfilePageComponent} from "./Components/profile-page/profile-page.component";
import {JwtInterceptor} from "./helpers/jwt.interceptor";
import {NgOptimizedImage} from "@angular/common";
import {CommentCreationDialogComponent} from "./Components/comment-creation-dialog/comment-creation-dialog.component";
import {MAT_DIALOG_DEFAULT_OPTIONS, MatDialog, MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {MatInputModule} from "@angular/material/input";
import {UserUpdateDialogComponent} from "./Components/user-update-dialog/user-update-dialog.component";
import {PostCreatorComponent} from "./Components/post-creator/post-creator.component";
import {AutoResizeInputComponent} from "./Components/auto-resize-input/auto-resize-input.component";
import {PostEditorDialogComponent} from "./Components/post-editor-dialog/post-editor-dialog.component";
import {UserHoverCardComponent} from "./Components/user-hover-card/user-hover-card.component";
import {FollowingPageComponent} from "./Components/following-page/following-page.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginPageComponent,
    SignUpPageComponent,
    NotifierComponent,
    MainPageComponent,
    PagePostComponent,
    ProfilePageComponent,
    CommentCreationDialogComponent,
    UserUpdateDialogComponent,
    PostCreatorComponent,
    AutoResizeInputComponent,
    PostEditorDialogComponent,
    UserHoverCardComponent,
    FollowingPageComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'counter', component: CounterComponent},
      {path: 'fetch-data', component: FetchDataComponent},
    ]),
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    FontAwesomeModule,
    NgOptimizedImage,
    MatDialogModule,
    MatButtonModule,
    MatInputModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi:true},
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}}
  ],
  bootstrap: [AppComponent],
  entryComponents: [CommentCreationDialogComponent]
})
export class AppModule {
}
