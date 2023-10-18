import { Injectable } from '@angular/core';
import {PageUserDTO} from "../models/user/pageUserDTO";

@Injectable({
  providedIn: 'root'
})
export class UserHoverCardTriggerService {

  isHoverCardVisible: boolean = false;
  isHoveredOnTriggeringElement: boolean = false;
  coordinates: {x: number, y: number} = {x: 10, y: 10};
  user: PageUserDTO = {} as PageUserDTO;
  isInsideHoverCard: boolean = false;

  disableHoverCardVisibility() {
    this.isHoverCardVisible = false;
  }

  enableHoverCardVisibility() {
    this.isHoverCardVisible = true;
  }
}
