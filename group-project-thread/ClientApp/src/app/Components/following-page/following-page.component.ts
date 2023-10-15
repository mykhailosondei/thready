import {Component, OnInit} from '@angular/core';
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserDTO} from "../../models/user/userDTO";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {finalize, Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-following-page',
  templateUrl: './following-page.component.html',
  styleUrls: ['./following-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowingPageComponent implements OnInit{
  protected username : string;
  protected user! : UserDTO;
  protected following : UserWithPostDTO[];
  private unsubscribe$ = new Subject<void>();
  protected isCurrentUser : boolean = false;


  constructor(private userService: UserService, private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
    this.following = [];
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
      this.checkIsCurrentUser();
      this.fetchFollowingData(this.username);
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  fetchFollowingData(username: string) {
    this.userService.getUserByUsername(this.username)
      .subscribe(response => {
        if (response.body != null) {
          this.user = response.body;
          this.following = [];
          for (let i = 0; i < this.user.followingIds.length; i++) {
            const userId = this.user.followingIds[i];
            this.userService.getUserById(userId)
              .subscribe(response => {
                if (response.body != null) {
                  const userResponse: UserDTO = response.body;
                  const followingUser: UserWithPostDTO = {
                    avatar: userResponse.avatar,
                    bio: userResponse.bio,
                    followers: userResponse.followersIds.length,
                    following: userResponse.followingIds.length,
                    id: userResponse.id,
                    username: userResponse.username,
                  };
                  this.following.push(followingUser);
                }
              });
          }
        }
      });
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
