import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http'
import { Observable, map, of } from 'rxjs';
import { HttpInternalService } from './http-internal.service';
import { UserService } from './user.service';
import { EventService } from './event.service';
import { AuthUserDTO } from '../models/auth/authUserDTO';
import { RegisterUserDTO } from '../models/auth/registerUserDTO';
import { LoginUserDTO } from '../models/auth/loginUserDTO';
import {UserDTO} from "../models/user/userDTO";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public routePrefix: string = '/api/auth';
  private user: UserDTO | null = null;

  constructor(private httpService : HttpInternalService, private userService : UserService,
    private eventService: EventService) { }

  public getUser() {
    return this.user ? of(this.user) : this.userService.getCurrentUser().pipe(
      map((response) => {
        this.user = response.body;
        this.eventService.userChanged(this.user);
        return this.user;
      } )
    )
  }

  public setUser(user: UserDTO){
    this.user = user;
    this.eventService.userChanged(user);
  }

  public register(user: RegisterUserDTO){
    return this.handleAuthResponse(this.httpService.postFullRequest<AuthUserDTO>(`${this.routePrefix}/register`, user))
  }
  public login(user: LoginUserDTO){
    return this.handleAuthResponse(this.httpService.postFullRequest<AuthUserDTO>(`${this.routePrefix}/login`, user))
  }
  private handleAuthResponse(observable: Observable<HttpResponse<AuthUserDTO>>) {
    return observable.pipe(
      map((resp) => {
        const authToken = resp.body?.authToken;
        const user = resp.body?.user;

        if (authToken && user) {
            this.setToken(authToken);
            this.user = user;
            this.eventService.userChanged(user);
            return user;
        }
        else{
          return null;
        }
      })
    );
  }

  private setToken(token: string){
    if (token){
      localStorage.setItem('authToken', JSON.stringify(token));
    }
  }
}
