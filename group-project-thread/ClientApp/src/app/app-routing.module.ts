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

const routes: Routes = [
  {path:'', redirectTo: '/login', pathMatch: 'full'},
  {path:'login', component: LoginPageComponent},
  {path:'signup', component: SignUpPageComponent},
  {path:'mainPage', component: MainPageComponent},
  {path: 'mainpage', redirectTo: '/mainPage', pathMatch: 'full'},
  {path: ':username/profile', component: ProfilePageComponent},
  {path: ':username/following', component: FollowingPageComponent},
  {path: ':username/followers', component: FollowersPageComponent},
  {path: ':username/post/:id', component: SingularPostViewComponent},
  {path: 'explore', component: MayBeInterestingPageComponent},
  {path: 'trending', component: TrendingPageComponent},
  {path: "search", component: SearchResultsPageComponent},
  {path: 'comment/:commentId', component: SingularCommentViewComponent},
  {path: 'bob', component: RecommendationsSideBarComponent},
  {path: 'bookmarks', component: BookmarksPageComponent},
  {path: 'reposts', component: RepostsPageComponent},
  {path: 'connect-people', component: WhoToFollowPageComponent},
  {path: 'creators-for-you', component: CreatorsForYouPageComponent},
  {path: 'photo', component: PhotoLoaderComponent},
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
