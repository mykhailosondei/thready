import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpInternalService} from "../../Services/http-internal.service";
import {AuthService} from "../../Services/auth.service";
import {Subject, takeUntil} from "rxjs";
import {UserDTO} from "../../models/user/userDTO";
import {Image} from "../../models/image";
import {SnackbarService} from "../../Services/snackbar.service";
import {UserService} from "../../Services/user.service";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit, OnDestroy{

  public user = {} as UserDTO | null;
  public loading = false;
  public image: Image | null = null;

  private unsubscribe$ = new Subject<void>;
  constructor(httpServise: HttpInternalService,
              private authService : AuthService,
              private snackBarService: SnackbarService,
              private userService : UserService) {

  }
  ngOnInit(): void {
    this.authService.getUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((user) => {
        if (user){
          this.user = this.userService.copyUser(user);
        }
      }, (error) => this.snackBarService.showErrorMessage(error))
  }

  ngOnDestroy(): void {

  }

  test() : void {
    this.authService.getUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((user) => {
        if (user){
          this.user = this.userService.copyUser(user);
        }
      }, (error) => this.snackBarService.showErrorMessage(error.error.title))
  }

}
