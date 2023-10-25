import {Injectable} from '@angular/core';
import {HttpResponse} from '@angular/common/http'
import {map, Observable, of} from 'rxjs';
import {HttpInternalService} from './http-internal.service';
import {UserService} from './user.service';
import {EventService} from './event.service';
import {AuthUserDTO} from '../models/auth/authUserDTO';
import {RegisterUserDTO} from '../models/auth/registerUserDTO';
import {LoginUserDTO} from '../models/auth/loginUserDTO';
import {UserDTO} from "../models/user/userDTO";
import {A} from "@angular/cdk/keycodes";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public routePrefix: string = '/api/auth';

  constructor(private httpService : HttpInternalService, private userService : UserService,
    private eventService: EventService) { }

  public register(user: RegisterUserDTO){
    return this.handleAuthResponse(this.httpService.postFullRequest<AuthUserDTO>(`${this.routePrefix}/register`, user))
  }
  public login(user: LoginUserDTO){
    return this.handleAuthResponse(this.httpService.postFullRequest<AuthUserDTO>(`${this.routePrefix}/login`, user))
  }
  private handleAuthResponse(observable: Observable<HttpResponse<AuthUserDTO>>) {
    return observable.pipe(
      map((resp) => {
        const authToken = resp.body?.token;
        const user = resp.body?.user;
        if (authToken && user) {
            this.setToken(authToken);
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

      localStorage.setItem('token', JSON.stringify(token));
    }
  }
}
