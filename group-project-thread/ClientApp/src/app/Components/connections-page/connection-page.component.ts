import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../Services/auth.service";
import {catchError, Subject, takeUntil} from "rxjs";
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {SnackbarService} from "../../Services/snackbar.service";

@Component({
  selector: 'app-connection-page',
  templateUrl: './connection-page.component.html',
  styleUrls: ['./connection-page.component.scss', '../../../assets/ContentFrame.scss']
})
export class ConnectionPageComponent implements OnInit{
  protected readonly faArrowLeftLong = faArrowLeftLong;
  public user! : UserDTO;

  @Input() public isFollowing : boolean;
  @Input() public isFollowers : boolean;
  @Input() public username : string;
  @Output() public emittedUser: EventEmitter<UserDTO> = new EventEmitter<UserDTO>();


  private unsubscribe$ = new Subject<void>;

  constructor(private userService: UserService, private router: Router, private snackBarService : SnackbarService ) {

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
        this.emittedUser.emit(this.user);
      }
    })
  }

  backToMainPaige() {
    this.router.navigate(["mainPage"]);
  }

  public navigateToFollowing(){
    this.router.navigate([this.user.username, "following"])
  }

  public navigateToFollowers(){
    this.router.navigate([this.user.username, "followers"])
  }
}

