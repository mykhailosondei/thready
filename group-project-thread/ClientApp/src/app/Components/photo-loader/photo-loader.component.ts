import {Component, EventEmitter, Input, Output} from '@angular/core';
import {HttpInternalService} from "../../Services/http-internal.service";
import {ImageUploadService} from "../../Services/image-upload.service";
import {faImage} from "@fortawesome/free-regular-svg-icons";
import {IconDefinition} from "@fortawesome/free-brands-svg-icons";

@Component({
  selector: 'app-photo-loader',
  templateUrl: './photo-loader.component.html',
  styleUrls: ['./photo-loader.component.scss']
})
export class PhotoLoaderComponent {

  constructor(private imageUploadService: ImageUploadService) {
  }

  @Input() disabled: boolean = false;
  @Output() imageUrlOutputEvent: EventEmitter<string> = new EventEmitter<string>();

  selectedFile: File | null = null;
  imageUrl: string | null = null;
  onFileSelected($event: Event) {
    this.selectedFile = ($event.target as HTMLInputElement).files![0];

    this.imageUploadService.uploadImage(this.selectedFile).subscribe((res) => {
      console.log(res.body!.url);
      this.imageUrl = res.body!.url;
      this.imageUrlOutputEvent.emit(this.imageUrl);
    });
  }


  selectFile() {
    if(this.disabled) return;
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = 'image/*';
    fileInput.style.display = 'none';

    fileInput.addEventListener('change', (event: Event) => {
      this.onFileSelected(event);
    });

    document.body.appendChild(fileInput);

    fileInput.click();
  }

  protected readonly faImage = faImage;

  get faImageIcon(): IconDefinition {
    var result = faImage;
    return faImage;
  }
}
