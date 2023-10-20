import { Injectable } from '@angular/core';
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class FollowingFollowersNavigatorService {
  constructor(private router : Router) { }

  public openProfilePage($event : string){
    this.router.navigate([$event, 'profile'])
  }
  public openFollowingPage($event : string){
    this.router.navigate([$event, 'following'])
  }
  public openFollowersPage($event : string){
    this.router.navigate([$event, 'followers'])
  }
}
