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
import {Endpoint} from "../side-navbar/side-navbar.component";

@Component({
  selector: 'app-who-to-follow-page',
  templateUrl: './who-to-follow-page.component.html',
  styleUrls: ['./who-to-follow-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class WhoToFollowPageComponent {

  protected readonly Tab = Tab;
  public users$ : BehaviorSubject<PageUserDTO[]> = new BehaviorSubject<PageUserDTO[]>([]);
  public currentUser! : UserDTO;
  loading: boolean;
  constructor(public navigator : NavigatorService,
              private recommendationService : RecommendationService,
              public userService : UserService, private historyOfPages : NavigationHistoryService,
              private location : Location) {
  }

  ngOnInit(){
    this.loading = true;
    this.userService.getCurrentUserInstance().subscribe(
      (user) => this.currentUser = user);
    this.getUsersToFollow();
  }

  getUsersToFollow(){
    this.recommendationService.getConnectPeople()
      .subscribe((response) =>
      {
        this.loading = false;
        if (response.body != null){
          this.users$.next(response.body)
        }
      })
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

  protected readonly Endpoint = Endpoint;
}
