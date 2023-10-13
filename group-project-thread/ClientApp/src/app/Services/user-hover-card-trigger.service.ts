import { Injectable } from '@angular/core';
import {UserWithPostDTO} from "../models/user/UserWithinPostDTO";

@Injectable({
  providedIn: 'root'
})
export class UserHoverCardTriggerService {

  isHoverCardVisible: boolean = false;
  coordiantes: {x: number, y: number} = {x: 10, y: 10};
  user: UserWithPostDTO = {} as UserWithPostDTO;
  isInsideHoverCard: boolean = false;

  disableHoverCardVisibility() {
    this.isHoverCardVisible = false;
  }

  enableHoverCardVisibility() {
    this.isHoverCardVisible = true;
  }
}
