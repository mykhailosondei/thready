import { Component } from '@angular/core';
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss', '../../../assets/ContentFrame.scss']
})
export class SearchBarComponent {

  protected readonly faArrowLeftLong = faArrowLeftLong;

  backToMainPaige() {

  }

  protected readonly faMagnifyingGlass = faMagnifyingGlass;
}
