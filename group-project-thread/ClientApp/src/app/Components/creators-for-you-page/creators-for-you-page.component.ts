import { Component } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {NavigatorService} from "../../Services/navigator.service";
import {RecommendationService} from "../../Services/recommendation.service";
import {UserService} from "../../Services/user.service";
import {Tab} from "../../models/enums/Tab";
import {NavigationHistoryService} from "../../Services/navigation-history.service";
import {Location} from "@angular/common";

@Component({
  selector: 'app-creators-for-you-page',
  templateUrl: './creators-for-you-page.component.html',
  styleUrls: ['./creators-for-you-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class CreatorsForYouPageComponent {
  protected readonly Tab = Tab;
  public users$ : BehaviorSubject<PageUserDTO[]> = new BehaviorSubject<PageUserDTO[]>([]);
  public currentUser! : UserDTO;
  loading: boolean;
  private navigateToMainPage: boolean;
  constructor(public navigator : NavigatorService,
              private recommendationService : RecommendationService,
              private userService : UserService, private historyOfPages : NavigationHistoryService,
              private location : Location) {
  }

  ngOnInit(){
    this.loading = true;
    this.getCurrentUser();
    this.getUsersToFollow();
  }

  getUsersToFollow(){
    this.recommendationService.getCreatorsForYou()
      .subscribe((response) =>
      {
        if (response.body != null){
          this.users$.next(response.body)
          this.loading = false;
        }
      })
  }
  getCurrentUser(): void{
    this.userService.getCurrentUser()
      .subscribe( (response) =>{
        if (response.body != null){
          this.currentUser = response.body;
        }
      });
  }

  navigateToWhoToFollow(){
    if (this.historyOfPages.getPageInHistoryCounter() == 0){
      this.navigateToMainPage =true;
    }
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigator.openWhoToFollowPage('');
  }

  goBack(){
    this.navigator.backToMainPage();
  }
}
