import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss', '../../../assets/spinner.scss']
})
export class LoadingSpinnerComponent {
  @Input() loading : boolean;
}
