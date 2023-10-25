import {Component, EventEmitter, Input, Output} from '@angular/core';
import {faArrowLeftLong} from "@fortawesome/free-solid-svg-icons";
import {faMagnifyingGlass} from "@fortawesome/free-solid-svg-icons/faMagnifyingGlass";
import {Tab} from "../../models/enums/Tab";
import {Router} from "@angular/router";
import {Location} from "@angular/common";
@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss', '../../../assets/ContentFrame.scss', '../../../assets/navigation-bar.scss']
})
export class SearchBarComponent{

  protected readonly faArrowLeftLong = faArrowLeftLong;
  protected readonly faMagnifyingGlass = faMagnifyingGlass;

  @Input() public selectedTab : Tab;
  @Input() public firstTabName : string;
  @Input() public secondTabName : string;
  @Input() public query : string  = "";

  @Output() searchByQueryClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() firstTabClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() secondTabClicked : EventEmitter<string> = new EventEmitter<string>();
  @Output() queryChanged: EventEmitter<string> = new EventEmitter<string>();
  @Output() backButtonClicked : EventEmitter<undefined> = new EventEmitter<undefined>();

  constructor() {
  }



  backToMainPaige() {
    this.backButtonClicked.emit();
  }

  public navigateToFirstTab(){
    this.firstTabClicked.emit();
  }

  public navigateToSecondTab(){
    this.secondTabClicked.emit();
  }

  searchByQuery() {
    this.queryChanged.emit(this.query);
    this.searchByQueryClicked.emit();
  }

}
