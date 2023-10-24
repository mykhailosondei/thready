import {Component} from '@angular/core';
import {UserHoverCardTriggerService} from "../../Services/user-hover-card-trigger.service";
import {animate, state, style, transition, trigger} from "@angular/animations";
import PostFormatter from "../../helpers/postFormatter";
import {UserService} from "../../Services/user.service";
import {Router} from "@angular/router";
import {NavigatorService} from "../../Services/navigator.service";

@Component({
  selector: 'app-user-hover-card',
  templateUrl: './user-hover-card.component.html',
  animations: [
    trigger('fadeInOut', [
      state('void', style({
        opacity: 0,
              })),
      state('*', style({
        opacity: 1,
      })),
      transition('void <=> *', animate('300ms ease-in-out')),
    ]),
  ],
  styleUrls: ['./user-hover-card.component.scss']
})
export class UserHoverCardComponent{
  constructor(private hoverCardTriggerService: UserHoverCardTriggerService,
              private readonly userService: UserService, public readonly navigator : NavigatorService) { }

  get user() { return this.hoverCardTriggerService.user; }

  isUserFollowed : boolean;
  isUserMyself : boolean;
  isFollowResponseSuccess : boolean;

  get showFollowButton() { return !this.isUserFollowed && !this.isUserMyself; }
  get following() {return PostFormatter.numberToReadable(this.user.following)}
  get followers() {return PostFormatter.numberToReadable(this.user.followers)}
  elementEntered() {
    console.log("entered");
    this.isFollowResponseSuccess = false;
    this.userService.getCurrentUser().subscribe(response => {
      if(response.ok) {
        this.isUserFollowed = response.body!.followingIds.includes(this.user.id);
        this.isUserMyself = response.body!.id === this.user.id;
        this.isFollowResponseSuccess = true;
      }
    });
  }

  contentVisible = false;

  get coordinates(): {x: number, y: number} { return this.hoverCardTriggerService.coordinates; }

  get isHoverCardVisible(): boolean {
    return this.hoverCardTriggerService.isHoverCardVisible;
  }

  protected readonly UserHoverCardTriggerService = UserHoverCardTriggerService;

  onMouseEnter() {
    this.hoverCardTriggerService.isInsideHoverCard = true;
  }
  onMouseLeave() {
    this.hoverCardTriggerService.disableHoverCardVisibility();
    this.hoverCardTriggerService.isInsideHoverCard = false;
  }

  getCircleColor() {
    return PostFormatter.getCircleColor(this.user.username);
  }

  isAvatarNull() {
    return this.user.avatar === null
  }

  getFirstInitial() {
    return this.user.username[0].toUpperCase();
  }

  followUser() {
    this.userService.followUser(this.user.id).subscribe(followResponse => {
      if(followResponse.ok){
        this.userService.getCurrentUser().subscribe(response => {
          if(response.ok) this.user.followers = response.body!.followingIds.length;
        });
        this.isUserFollowed = true;
      }
    });
  }

  unfollowUser() {
    this.userService.unfollowUser(this.user.id).subscribe(unfollowResponse => {
      if(unfollowResponse.ok){
        this.userService.getCurrentUser().subscribe(response => {
          if(response.ok) this.user.followers = response.body!.followingIds.length;
        });
        this.isUserFollowed = false;
      }
    });
  }
}
