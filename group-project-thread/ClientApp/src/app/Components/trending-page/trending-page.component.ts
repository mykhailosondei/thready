import { Component } from '@angular/core';
import {Tab} from "../../models/enums/Tab";
import {BehaviorSubject, Subject, takeUntil} from "rxjs";
import {PostDTO} from "../../models/post/postDTO";
import {UserDTO} from "../../models/user/userDTO";
import {Router} from "@angular/router";
import {RecommendationService} from "../../Services/recommendation.service";
import {UserService} from "../../Services/user.service";
import {NavigatorService} from "../../Services/navigator.service";
import {IndexedWordDTO} from "../../models/indexedWordDTO";
import {NavigationHistoryService} from "../../Services/navigation-history.service";
import {Location} from "@angular/common";

@Component({
  selector: 'app-trending-page',
  templateUrl: './trending-page.component.html',
  styleUrls: ['./trending-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class TrendingPageComponent {
  public selectedTab : Tab = 1;
  public query : string;
  public trends$ : BehaviorSubject<IndexedWordDTO[]> = new BehaviorSubject<IndexedWordDTO[]>([]);
  public currentUser! : UserDTO;
  constructor(private router : Router, private recommendationService : RecommendationService,
              private userService : UserService, public navigator : NavigatorService,
              private historyOfPages : NavigationHistoryService, private location : Location) {
  }

  ngOnInit(): void {
    this.getCurrentUser();
    this.getTrends();
  }

  getTrends() {
        this.recommendationService.getTrends()
          .subscribe((response) => {
            if (response.body != null) {
              this.trends$.next(response.body);
            }
          });
  }

  getCurrentUser(){
    this.userService.getCurrentUser()
      .subscribe((response) => {
        if (response.body != null){
          this.currentUser = response.body;
        }
      } )
  }

  navigateToMayBeInteresting(){
    if (this.historyOfPages.getPageInHistoryCounter() == 0){
      this.historyOfPages.setNavigateToMainPage();
    }
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigator.navigateToMayBeInterestingPage(Tab.FirstTab);
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
