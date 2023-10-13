import { Component } from '@angular/core';
import {UserHoverCardTriggerService} from "../../Services/user-hover-card-trigger.service";
import {animate, state, style, transition, trigger} from "@angular/animations";

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
export class UserHoverCardComponent {
  constructor(private hoverCardTriggerService: UserHoverCardTriggerService) {

  }

  get user() { return this.hoverCardTriggerService.user; }

  get coordinates(): {x: number, y: number} { return this.hoverCardTriggerService.coordiantes; }

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
}
