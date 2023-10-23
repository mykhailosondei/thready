import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-no-result-found-page',
  templateUrl: './no-result-found-page.component.html',
  styleUrls: ['./no-result-found-page.component.scss']
})
export class NoResultFoundPageComponent {
  @Input() query : string;
}
