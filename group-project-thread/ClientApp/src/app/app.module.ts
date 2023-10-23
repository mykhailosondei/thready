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
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
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
import {OnDomEnterDirective} from "./Directives/on-dom-enter.directive";
import {SingularPostViewComponent} from "./Components/singular-post-view/singular-post-view.component";
import {FollowingPageComponent} from "./Components/following-page/following-page.component";
import {FollowersPageComponent} from "./Components/followers-page/followers-page.component";
import {PageUserComponent} from "./Components/page-user/page-user.component";
import {SearchBarComponent} from "./Components/search-bar/search-bar.component";
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {SearchResultsPageComponent} from "./Components/search-results-page/search-results-page.component";
import {CommentCreatorComponent} from "./Components/comment-creator/comment-creator.component";
import {PageCommentComponent} from "./Components/page-comment/page-comment.component";
import {SingularCommentViewComponent} from "./Components/singular-comment-view/singular-comment-view.component";
import {TopBlurryBarComponent} from "./Components/top-blurry-bar/top-blurry-bar.component";
import {
  RecommendationsSideBarComponent
} from "./Components/recommendations-side-bar/recommendations-side-bar.component";
import {TablistPageComponent} from "./Components/tablist-page/tablist-page.component";
import {SideNavbarComponent} from "./Components/side-navbar/side-navbar.component";
import {PostCreationDialogComponent} from "./Components/post-creation-dialog/post-creation-dialog.component";
import {BookmarksPageComponent} from "./Components/bookmarks-page/bookmarks-page.component";
import {RepostsPageComponent} from "./Components/reposts-page/reposts-page.component";
import {WhoToFollowPageComponent} from "./Components/who-to-follow-page/who-to-follow-page.component";
import {CreatorsForYouPageComponent} from "./Components/creators-for-you-page/creators-for-you-page.component";
import {MayBeInterestingPageComponent} from "./Components/may-be-interesting-page/may-be-interesting-page.component";
import {TrendingPageComponent} from "./Components/trending-page/trending-page.component";
import {PhotoLoaderComponent} from "./Components/photo-loader/photo-loader.component";
import {DeletableImageComponent} from "./Components/deletable-image/deletable-image.component";
import {LoadingSpinnerComponent} from "./Components/loading-spinner/loading-spinner.component";
import {NoResultFoundPageComponent} from "./Components/no-result-found-page/no-result-found-page.component";

@NgModule({ declarations: [
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
    FollowingPageComponent,
    FollowersPageComponent,
    UserHoverCardComponent,
    OnDomEnterDirective,
    SingularPostViewComponent,
    SearchBarComponent,
    SearchResultsPageComponent,
    CommentCreatorComponent,
    PageCommentComponent,
    SingularCommentViewComponent,
    TopBlurryBarComponent,
    SideNavbarComponent,
    PostCreationDialogComponent,
    BookmarksPageComponent,
    RepostsPageComponent,
    PageUserComponent,
    RecommendationsSideBarComponent,
    TablistPageComponent,
    FollowersPageComponent,
    WhoToFollowPageComponent,
    CreatorsForYouPageComponent,
    MayBeInterestingPageComponent,
    TrendingPageComponent,
    PhotoLoaderComponent,
    DeletableImageComponent,
    LoadingSpinnerComponent,
    NoResultFoundPageComponent
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
    MatInputModule,
    InfiniteScrollModule,
    MatProgressSpinnerModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi:true},
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false},
    }
  ],
  bootstrap: [AppComponent],
  entryComponents: [CommentCreationDialogComponent]
})

export class AppModule {
}
