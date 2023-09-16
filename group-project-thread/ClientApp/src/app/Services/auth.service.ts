import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { UserDTO } from '../models/userDTO';
import { map, of } from 'rxjs';
import { HttpInternalService } from './http-internal.service';
import { UserService } from './user.service';
import { EventService } from './event.service';
import { AuthUserDTO } from '../models/auth/authUserDTO';
import { RegisterUserDTO } from '../models/auth/registerUserDTO';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public routePrefix: string = '/api/AuthController';
  private user: UserDTO | null = null;

  constructor(private httpService : HttpInternalService, private userService : UserService,
    private eventService: EventService) { }

  public getUser() {
    return this.user ? of(this.user) : this.userService.getCurrentUser().pipe(
      map((responce) => {
        this.user = responce.body;
        this.eventService.userChanged(this.user);
        return this.user;
      } )
    )
  }

  public setUser(user: UserDTO){
    this.user = user;
    this.eventService.userChanged(user);
  }

  
}
