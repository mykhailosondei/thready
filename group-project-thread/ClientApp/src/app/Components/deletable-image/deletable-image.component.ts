import {Component, EventEmitter, Input, Output} from '@angular/core';
import {faTimes} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-deletable-image',
  templateUrl: './deletable-image.component.html',
  styleUrls: ['./deletable-image.component.scss']
})
export class DeletableImageComponent {
  @Input() imageUrl: string;
  @Output() deleteImage: EventEmitter<string> = new EventEmitter<string>();

  closeImage() {
    this.deleteImage.emit(this.imageUrl);
  }

  protected readonly faTimes = faTimes;
}
