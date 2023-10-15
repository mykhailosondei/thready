import {Component, NgZone} from '@angular/core';
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {finalize, Subject, takeUntil} from "rxjs";
@Component({
  selector: 'app-followers-page',
  templateUrl: './followers-page.component.html',
  styleUrls: ['./followers-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowersPageComponent {
  protected username : string;
  protected user! : UserDTO;
  protected followers : UserWithPostDTO[];
  private unsubscribe$ = new Subject<void>();
  protected isCurrentUser : boolean = false;
  constructor(private userService: UserService, private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
    this.followers = [];
  }

  handleUser(user: UserDTO) {
    this.user = user;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
      this.checkIsCurrentUser();
      this.fetchFollowersData(this.username);
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  fetchFollowersData(username: string) {
    this.userService.getUserByUsername(username).subscribe(response => {
      if (response.body != null) {
        this.user = response.body;
        this.followers = [];
        for (let i = 0; i < this.user.followersIds.length; i++) {
          const userId = this.user.followersIds[i];
          this.userService.getUserById(userId).subscribe(response => {
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

  amIFollowing(id : number): boolean{
    return this.user.followingIds.includes(id);

  }

  checkIsCurrentUser(): void{
    this.userService.getCurrentUser().pipe(takeUntil(this.unsubscribe$))
      .subscribe( (response) =>{
      if (response.body != null){
        this.isCurrentUser = this.username == response.body.username;
      }
    });
  }
}
