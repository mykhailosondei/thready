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

  @Input() public isFollowing : boolean;
  @Input() public isFollowers : boolean;
  @Input() public username : string;

  constructor(private router: Router) {

  }

  ngOnInit(): void {

  }

  backToMainPaige() {
    this.router.navigate(["mainPage"]);
  }

  public navigateToFollowing(){
    this.router.navigate([this.username, "following"])
  }

  public navigateToFollowers(){
    this.router.navigate([this.username, "followers"])
  }

  navigateToProfile(username: string) {
    this.router.navigate([this.username, "profile"])
  }
}

