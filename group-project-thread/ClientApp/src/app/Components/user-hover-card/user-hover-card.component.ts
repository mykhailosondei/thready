import {Component, OnInit} from '@angular/core';
import {UserHoverCardTriggerService} from "../../Services/user-hover-card-trigger.service";
import {animate, state, style, transition, trigger} from "@angular/animations";
import PostFormatter from "../../helpers/postFormatter";

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
export class UserHoverCardComponent implements OnInit{
  constructor(private hoverCardTriggerService: UserHoverCardTriggerService) {

  }

  get user() { return this.hoverCardTriggerService.user; }

  get following() {return PostFormatter.numberToReadable(this.user.following)}
  get followers() {return PostFormatter.numberToReadable(this.user.followers)}
  ngOnInit(): void {

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
}
