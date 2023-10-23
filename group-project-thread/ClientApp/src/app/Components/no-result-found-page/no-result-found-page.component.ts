import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-no-result-found-page',
  templateUrl: './no-result-found-page.component.html',
  styleUrls: ['./no-result-found-page.component.scss']
})
export class NoResultFoundPageComponent {
  @Input() headerText : string;
  @Input() smallText : string;
  public showNoResults = false;

  ngOnInit() {
    // Add a delay of 2000 milliseconds (2 seconds) before showing the element.
    setTimeout(() => {
      this.showNoResults = true;
    }, 1);
  }
}
