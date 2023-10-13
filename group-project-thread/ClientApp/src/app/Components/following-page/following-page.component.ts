import {Component, Input, OnInit} from '@angular/core';
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../Services/auth.service";
import {catchError, Subject, takeUntil} from "rxjs";
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {SnackbarService} from "../../Services/snackbar.service";

@Component({
  selector: 'app-following-page',
  templateUrl: './following-page.component.html',
  styleUrls: ['./following-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class FollowingPageComponent implements OnInit{
  protected readonly faArrowLeftLong = faArrowLeftLong;
  public user!: UserDTO;
  public username : string;

  private unsubscribe$ = new Subject<void>;

  constructor(private userService: UserService, private route: ActivatedRoute, private authService: AuthService,
              private router: Router, private  snackBarService : SnackbarService ) {
    this.route.paramMap.subscribe(params => {
      this.username = params.get('username') || "DefaultUsername";
    })
  }

  ngOnInit(): void {
    this.userService.getUserByUsername(this.username)
      .pipe(takeUntil(this.unsubscribe$), catchError(error => {
        this.snackBarService.showErrorMessage(error.error.title);
        this.backToMainPaige();
        throw error;
      })).subscribe( response =>
    {
      if (response.body != null){
        this.user  = response.body;
      }
    })
  }


  backToMainPaige() {
    this.router.navigate(["mainPage"]);
  }
}
