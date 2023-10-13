import {Injectable} from '@angular/core';
import {HttpInternalService} from './http-internal.service';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpResponse} from '@angular/common/http';
import {UserDTO} from "../models/user/userDTO";
import {UserUpdateDTO} from "../models/user/userUpdateDTO";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public routePrefix: string = '/api/user';
  constructor(private httpService : HttpInternalService) { }

  public getAllUsers() : Observable<HttpResponse<UserDTO[]>>{
    return this.httpService.getFullRequest<UserDTO[]>(`${this.routePrefix}`);
  }

  public getUserById(id: number) : Observable<HttpResponse<UserDTO>>{
    return this.httpService.getFullRequest<UserDTO>(`${this.routePrefix}/${id}`);
  }

  public getUserByUsername(username: string) : Observable<HttpResponse<UserDTO>>{
    return this.httpService.getFullRequest<UserDTO>(`${this.routePrefix}/username/${username}`);
  }

  public getCurrentUser() : Observable<HttpResponse<UserDTO>>{
    return this.httpService.getFullRequest<UserDTO>(`${this.routePrefix}/currentUser`);
  }

  public followUser(id: number): Observable<HttpResponse<void>>{
    return this.httpService.postFullRequest<void>(`${this.routePrefix}/${id}/follow`, {});
  }

  public unfollowUser(id: number): Observable<HttpResponse<void>>{
    return this.httpService.postFullRequest<void>(`${this.routePrefix}/${id}/unfollow`, {});
  }

  public putUser(id: number, user : UserUpdateDTO): Observable<HttpResponse<UserDTO>>{
    return this.httpService.putFullRequest<UserDTO>(`${this.routePrefix}/${id}`, user);
  }

  public deleteUser(id: number,): Observable<HttpResponse<void>>{
    return this.httpService.deleteFullRequest<void>(`${this.routePrefix}/${id}`);
  }

  public copyUser({id, username, email, dateOfBirth, password, imageID, avatar, posts, postsCount, followersIds,
    followingIds, bio, location, bookmarkedPostsIds, repostsIds}: UserDTO){
    return {
      id,
      username,
      email,
      dateOfBirth,
      password,
      imageID,
      avatar,
      posts,
      postsCount,
      followersIds,
      followingIds,
      bio,
      location,
      bookmarkedPostsIds,
      repostsIds
    }
  }
}

