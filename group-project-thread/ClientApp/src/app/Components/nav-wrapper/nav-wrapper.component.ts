import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Endpoint} from "../side-navbar/side-navbar.component";
import {NavigatorService} from "../../Services/navigator.service";

@Component({
  selector: 'app-nav-wrapper',
  templateUrl: './nav-wrapper.component.html',
  styleUrls: ['./nav-wrapper.component.scss']
})
export class NavWrapperComponent {

  @Input() activatedEndpoint : Endpoint = Endpoint.Home;
  @Input() showSearchBar : boolean = true;
  @Input() showWhatsHappening : boolean = true;
  @Input() showWhoToFollow : boolean = true;

  @Output() trendClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() userClicked : EventEmitter<string> = new EventEmitter<string>();

constructor(private navigatorService : NavigatorService) {
}
  navigateToUserPage(username : string) {
    this.userClicked.emit(username);
  }
  searchByWord(word: string) {
    this.trendClicked.emit(word);
    this.navigatorService.searchByWord(word);
  }
}
