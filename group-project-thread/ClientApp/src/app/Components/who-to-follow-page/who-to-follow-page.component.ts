import { Component } from '@angular/core';
import {Tab} from "../../models/enums/Tab";
import {NavigatorService} from "../../Services/navigator.service";
import {RecommendationService} from "../../Services/recommendation.service";
import {BehaviorSubject} from "rxjs";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserService} from "../../Services/user.service";
import {UserDTO} from "../../models/user/userDTO";
import {NavigationHistoryService} from "../../Services/navigation-history.service";
import {Location} from "@angular/common";

@Component({
  selector: 'app-who-to-follow-page',
  templateUrl: './who-to-follow-page.component.html',
  styleUrls: ['./who-to-follow-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class WhoToFollowPageComponent {

  protected readonly Tab = Tab;
  public users$ : BehaviorSubject<PageUserDTO[]> = new BehaviorSubject<PageUserDTO[]>([]);
  public currentUser! : UserDTO;
  constructor(public navigator : NavigatorService,
              private recommendationService : RecommendationService,
              private userService : UserService, private historyOfPages : NavigationHistoryService,
              private location : Location) {
  }

  ngOnInit(){
    this.getCurrentUser();
    this.getUsersToFollow();
  }

  getUsersToFollow(){
    this.recommendationService.getWhoToFollow()
      .subscribe((response) =>
      {
        if (response.body != null){
          this.users$.next(response.body)
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

  navigateToCreatorsForYou(){
    if (this.historyOfPages.getPageInHistoryCounter() == 0){
      this.historyOfPages.setNavigateToMainPage();
    }
    this.historyOfPages.IncrementPageInHistoryCounter();
    this.navigator.openCreatorsForYouPage('');
  }

  goBack(){
    this.navigator.backToMainPage();
  }

}
