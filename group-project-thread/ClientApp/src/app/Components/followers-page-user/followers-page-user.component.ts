import {Component, Input} from '@angular/core';
import {PageUserDTO} from "../../models/user/pageUserDTO";
import {finalize, Subject, takeUntil} from "rxjs";
import {UserService} from "../../Services/user.service";
import {SnackbarService} from "../../Services/snackbar.service";
import {Router} from "@angular/router";
import {UserDTO} from "../../models/user/userDTO";

@Component({
  selector: 'app-followers-page-user',
  templateUrl: './followers-page-user.component.html',
  styleUrls: ['./followers-page-user.component.scss', '../../../assets/PageUser.scss']
})
export class FollowersPageUserComponent {
  @Input() public pageUser : PageUserDTO;
  @Input() public isCurrentUser : boolean;
  @Input() public isFollowing: boolean;
  @Input() public currentUser: UserDTO;
  @Input() public isMyPageProfile : boolean = false;


  protected isMyFollowing : boolean;
  public followingText : string = "Following";
  private submittedUnfollow: boolean = false;
  private submittedFollow : boolean = false;
  protected followed = false;
  constructor(private userService : UserService, private snackBarService : SnackbarService,
              private router: Router) {
  }


  navigateToUserProfile(username : string) {
    this.router.navigate([username, 'profile']);
  }

  amIFollowing(): boolean{
    if (this.isFollowing){
      this.followed = true;
      return true;
    }
    this.followed = false;
    return false;
  }

  changeSpan() {

    if (this.followingText == "Following"){
      this.followingText = "Unfollow"
    }
    else {
      this.followingText = "Following";
    }
  }
  unfollow(event:Event, id : number) {
    event.stopPropagation()
    if (!this.submittedUnfollow){
      this.submittedUnfollow = true;
      this.userService.unfollowUser(id).
      pipe( finalize(() => {
        this.submittedUnfollow = false;
      })).subscribe((responce) => {
        if (responce.ok){
          this.followed = false;
          const index = this.currentUser.followingIds.indexOf(id);
          this.currentUser.followingIds.splice(index, 1);

        }
      }, (error)=> {
        this.snackBarService.showErrorMessage(error);});
    }
  }


  follow(event: Event, id : number) {
    event.stopPropagation();
    if (!this.submittedFollow){
      this.submittedFollow = true;
      this.userService.followUser(id).
      pipe(finalize(() => this.submittedFollow = false ))
        .subscribe((responce) => {
          if (responce.ok){
            this.followed = true;
            this.currentUser.followingIds.push(id);
          }
        }, (error)=> {
          this.snackBarService.showErrorMessage(error.error.title);});
    }
  }

  getUserFollowers(event : Event, username: string) {
    event.stopPropagation();
    this.router.navigate([username, 'followers'])
  }
}
