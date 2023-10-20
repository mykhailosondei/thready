import { Component } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {UserDTO} from "../../models/user/userDTO";
import {FollowingFollowersNavigatorService} from "../../Services/following-followers-navigator.service";
import {RecommendationService} from "../../Services/recommendation.service";
import {UserService} from "../../Services/user.service";
import {Tab} from "../../models/enums/Tab";

@Component({
  selector: 'app-creators-for-you-page',
  templateUrl: './creators-for-you-page.component.html',
  styleUrls: ['./creators-for-you-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class CreatorsForYouPageComponent {
  protected readonly Tab = Tab;
  public users$ : BehaviorSubject<PageUserDTO[]> = new BehaviorSubject<PageUserDTO[]>([]);
  public currentUser! : UserDTO;
  constructor(public navigator : FollowingFollowersNavigatorService,
              private recommendationService : RecommendationService,
              private userService : UserService) {
  }

  ngOnInit(){
    this.getCurrentUser();
    this.getUsersToFollow();
  }

  getUsersToFollow(){
    this.recommendationService.getCreatorsForYou()
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
}
