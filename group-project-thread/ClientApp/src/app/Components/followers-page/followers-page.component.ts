import { Component } from '@angular/core';
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../Services/auth.service";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {finalize, Subject, takeUntil} from "rxjs";
import {C, F} from "@angular/cdk/keycodes";

@Component({
  selector: 'app-followers-page',
  templateUrl: './followers-page.component.html',
  styleUrls: ['./followers-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowersPageComponent {
  protected username : string;
  protected user! : UserDTO;
  protected followers : UserWithPostDTO[];
  protected isMyFollowing : boolean;
  public followingText: string = "Unfollow";
  private submittedUnfollow: boolean = false;
  private submittedFollow : boolean = false;
  private unsubscribe$ = new Subject<void>();
  protected followed = false;
  constructor(private userService: UserService, private route: ActivatedRoute,
              private  snackBarService : SnackbarService ) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
    this.followers = [];
  }

  handleUser(user: UserDTO) {
    this.user = user;
  }
  ngOnInit(): void {
    this.userService.getUserByUsername(this.username)
      .subscribe(response => {
        if (response.body != null) {
          this.user = response.body;
          for (let i = 0; i < this.user.followersIds.length; i++) {
            const userId = this.user.followersIds[i];
            this.userService.getUserById(userId)
              .subscribe(response => {
                if (response.body != null) {
                  const userResponse: UserDTO = response.body;
                  const follower: UserWithPostDTO = {
                    avatar: userResponse.avatar,
                    bio: userResponse.bio,
                    followers: userResponse.followersIds.length,
                    following: userResponse.followingIds.length,
                    id: userResponse.id,
                    username: userResponse.username,
                  };
                  this.followers.push(follower);
                }
              });
          }
        }
      });
  }

  navigateToUserProfile() {

  }

  amIFollowing(id : number): boolean{
    if (this.user.followingIds.includes(id)){
      this.followed = true;
      this.followingText = "Following";
      return true;
    }
    return false;
  }

  changeSpan() {
    if (this.followingText === "Unfollow"){
      this.followingText = "Following";
    }
    else {
      this.followingText = "Unfollow";
    }
  }
  unfollow(id : number) {
    this.submittedUnfollow = true;
    this.userService.unfollowUser(id).
    pipe(takeUntil(this.unsubscribe$), finalize(() => {
      this.submittedUnfollow = true;
    })).subscribe((responce) => {
      if (responce.ok){
        this.followed = false;
      }
    }, (error)=> {
      this.snackBarService.showErrorMessage(error.error.title);});
  }

  follow(id : number) {
    this.submittedFollow = true;
    this.userService.followUser(id).
    pipe(takeUntil(this.unsubscribe$), finalize(() => {
      this.submittedFollow = true;
    })).subscribe((responce) => {
      if (responce.ok){
        this.followed = true;
      }
    }, (error)=> {
      this.snackBarService.showErrorMessage(error.error.title);});
  }

}
