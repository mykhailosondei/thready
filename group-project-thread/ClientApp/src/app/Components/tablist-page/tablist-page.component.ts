import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UserDTO} from "../../models/user/userDTO";
import {UserService} from "../../Services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../Services/auth.service";
import {catchError, Subject, takeUntil} from "rxjs";
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {SnackbarService} from "../../Services/snackbar.service";
import {Tab} from "../../models/enums/Tab";

@Component({
  selector: 'app-tablist-page',
  templateUrl: './tablist-page.component.html',
  styleUrls: ['./tablist-page.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/navigation-bar.scss']
})
export class TablistPageComponent implements OnInit{
  protected readonly faArrowLeftLong = faArrowLeftLong;

  @Input() public selectedTab : Tab;
  @Input() public firstTabName : string;
  @Input() public secondTabName : string;
  @Input() public title : string;
  @Input() public showSubtitle : boolean = true;

  @Output() titleClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() firstTabClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() secondTabClicked : EventEmitter<string> = new EventEmitter<string>();

  constructor(private router: Router) {

  }

  ngOnInit(): void {

  }

  backToMainPaige() {
    this.router.navigate(["mainPage"]);
  }

  public navigateToFirstTab(){
    this.firstTabClicked.emit(this.title);
  }

  public navigateToSecondTab(){
    this.secondTabClicked.emit(this.title);
  }

  navigateToTitle(username: string) {
    this.titleClicked.emit(this.title);
  }

  protected readonly Tab = Tab;
}

