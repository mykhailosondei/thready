import {Component, Input} from '@angular/core';
import {Endpoint} from "../side-navbar/side-navbar.component";

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
}
