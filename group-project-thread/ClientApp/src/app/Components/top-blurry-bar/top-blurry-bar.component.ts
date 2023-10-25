import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-top-blurry-bar',
  templateUrl: './top-blurry-bar.component.html',
  styleUrls: ['./top-blurry-bar.component.scss']
})
export class TopBlurryBarComponent {
  @Input() title: string = "";
}
