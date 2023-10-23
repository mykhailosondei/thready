import {Component, OnInit} from '@angular/core';
import {Tab} from "../../models/enums/Tab";
import {Router} from "@angular/router";
import {RecommendationService} from "../../Services/recommendation.service";
import {BehaviorSubject, Subject} from "rxjs";
import {PostDTO} from "../../models/post/postDTO";
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {NavigatorService} from "../../Services/navigator.service";
import {NavigationHistoryService} from "../../Services/navigation-history.service";
import {Location} from "@angular/common";

@Component({
  selector: 'app-may-be-interesting-page',
  templateUrl: './may-be-interesting-page.component.html',
  styleUrls: ['./may-be-interesting-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class MayBeInterestingPageComponent implements OnInit{
  public selectedTab : Tab = 0;
  public query : string;
  public matchingPosts$ : BehaviorSubject<PostDTO[]> = new BehaviorSubject<PostDTO[]>([]);
  public currentUser! : UserDTO;

  public currentUserId$ = new BehaviorSubject<number | undefined>(undefined);
  private navigateToMainPage: boolean;
  constructor(private router : Router, private recommendationService : RecommendationService,
              private userService : UserService, public navigator : NavigatorService,
              private historyOfPages : NavigationHistoryService, private location : Location) {
  }

  ngOnInit(): void {
    this.getCurrentUser();
    this.getPostsForYou();
  }

  getPostsForYou() {
    this.currentUserId$.subscribe((currentUserId) => {
      if (currentUserId !== undefined) {
        this.recommendationService.getPostsForYou(currentUserId)
          .subscribe((response) => {
            if (response.body != null) {
              this.matchingPosts$.next(response.body);
            }
          });
      }
    });
  }

  getCurrentUser(){
    this.userService.getCurrentUser()
      .subscribe((response) => {
        if (response.body != null){
          this.currentUser = response.body;
          this.currentUserId$.next(this.currentUser.id);
        }
      } )
  }

  navigateToTrending(){
    if (this.historyOfPages.getPageInHistoryCounter() == 0){
      this.navigateToMainPage =true;
    }
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigator.navigateToTrendingPage(Tab.SecondTab);
  }

  goBack(){
    this.navigator.backToMainPage();
  }

  searchByQuery() {
    this.router.navigate(['search'], {queryParams : {q : this.query}})
  }

  onQueryChanged(query: string) {
    this.query = query;
  }
}
