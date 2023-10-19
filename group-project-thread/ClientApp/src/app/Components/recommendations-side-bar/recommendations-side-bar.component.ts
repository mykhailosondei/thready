import { Component } from '@angular/core';
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";

@Component({
  selector: 'app-recommendations-side-bar',
  templateUrl: './recommendations-side-bar.component.html',
  styleUrls: ['./recommendations-side-bar.component.scss', '../../../assets/PageUser.scss']
})
export class RecommendationsSideBarComponent {

  protected readonly faMagnifyingGlass = faMagnifyingGlass;
}
