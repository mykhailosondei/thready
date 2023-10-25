import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {LoginPageComponent} from './Components/login-page/login-page.component';
import {SignUpPageComponent} from './Components/sign-up-page/sign-up-page.component';
import {MainPageComponent} from "./Components/main-page/main-page.component";
import {ProfilePageComponent} from "./Components/profile-page/profile-page.component";
import {SingularPostViewComponent} from "./Components/singular-post-view/singular-post-view.component";
import {FollowingPageComponent} from "./Components/following-page/following-page.component";
import {FollowersPageComponent} from "./Components/followers-page/followers-page.component";
import {SearchBarComponent} from "./Components/search-bar/search-bar.component";
import {SearchResultsPageComponent} from "./Components/search-results-page/search-results-page.component";
import {SingularCommentViewComponent} from "./Components/singular-comment-view/singular-comment-view.component";
import {
  RecommendationsSideBarComponent
} from "./Components/recommendations-side-bar/recommendations-side-bar.component";
import {BookmarksPageComponent} from "./Components/bookmarks-page/bookmarks-page.component";
import {RepostsPageComponent} from "./Components/reposts-page/reposts-page.component";
import {WhoToFollowPageComponent} from "./Components/who-to-follow-page/who-to-follow-page.component";
import {CreatorsForYouPageComponent} from "./Components/creators-for-you-page/creators-for-you-page.component";
import {MayBeInterestingPageComponent} from "./Components/may-be-interesting-page/may-be-interesting-page.component";
import {TrendingPageComponent} from "./Components/trending-page/trending-page.component";
import {PhotoLoaderComponent} from "./Components/photo-loader/photo-loader.component";
import {AuthGuard} from "./Guards/AuthGuard";

const routes: Routes = [
  {path:'', redirectTo: '/login', pathMatch: 'full'},
  {path:'login', component: LoginPageComponent},
  {path:'signup', component: SignUpPageComponent},
  {path:'mainPage', component: MainPageComponent, canActivate: [AuthGuard]},
  {path: 'mainpage', redirectTo: '/mainPage', pathMatch: 'full'},
  {path: ':username/profile', component: ProfilePageComponent, canActivate: [AuthGuard]},
  {path: ':username/following', component: FollowingPageComponent, canActivate: [AuthGuard]},
  {path: ':username/followers', component: FollowersPageComponent, canActivate: [AuthGuard]},
  {path: ':username/post/:id', component: SingularPostViewComponent, canActivate: [AuthGuard]},
  {path: 'explore', component: MayBeInterestingPageComponent, canActivate: [AuthGuard]},
  {path: 'trending', component: TrendingPageComponent, canActivate: [AuthGuard]},
  {path: "search", component: SearchResultsPageComponent, canActivate: [AuthGuard]},
  {path: 'comment/:commentId', component: SingularCommentViewComponent, canActivate: [AuthGuard]},
  {path: 'bookmarks', component: BookmarksPageComponent, canActivate: [AuthGuard]},
  {path: 'reposts', component: RepostsPageComponent, canActivate: [AuthGuard]},
  {path: 'connect-people', component: WhoToFollowPageComponent, canActivate: [AuthGuard]},
  {path: 'creators-for-you', component: CreatorsForYouPageComponent, canActivate: [AuthGuard]},
  {path: 'photo', component: PhotoLoaderComponent, canActivate: [AuthGuard]},
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
