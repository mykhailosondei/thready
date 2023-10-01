import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import {UserDTO} from "../models/user/userDTO";

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private onUserChanged = new Subject<UserDTO | null>();
  public userChangedEvent$ = this.onUserChanged.asObservable();

  public userChanged(user: UserDTO | null) : void {
    this.onUserChanged.next(user);
  }

  constructor() { }
}
