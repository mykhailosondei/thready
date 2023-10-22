import {Component, Inject, NgModule, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UpdateUserDialogData} from "../../models/user/updateUserDialogData";
import {faCamera, faXmark} from "@fortawesome/free-solid-svg-icons";
import {Image} from "../../models/image";
import DevExpress from "devextreme/bundles/dx.all";
import data = DevExpress.data;
import PostFormatter from "../../helpers/postFormatter";
import {ImageUploadService} from "../../Services/image-upload.service";


@Component({
  selector: 'app-user-update-dialog',
  templateUrl: './user-update-dialog.component.html',
  styleUrls: ['./user-update-dialog.component.scss']
})
export class UserUpdateDialogComponent implements OnInit{
  faCross = faXmark;
  imageUrl: string = this.data.currentUser.avatar!.url;
  faXmark = faXmark;
  faCamere = faCamera;
  inputLocationFocused: boolean = false;
  inputBioFocused: boolean = false;
  charCountLocation: number = this.data.location.length;
  maxCharCountLocation: number = 30;
  charCountBio: number = this.data.bio.length;
  maxCharCountBio: number = 100;
  private selectedFile: File;

  constructor(public dialogRef: MatDialogRef<UserUpdateDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: UpdateUserDialogData,
              private imageUploadService: ImageUploadService) {
  }
  ngOnInit(): void {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  changeImage(){
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

  onFileSelected($event: Event) {
    this.selectedFile = ($event.target as HTMLInputElement).files![0];
    this.imageUploadService.uploadImage(this.selectedFile).subscribe((res) => {
      console.log(res.body!.url);
      this.imageUrl = res.body!.url;
    });
  }

  onInputBioFocus() {
    this.inputBioFocused = true;
  }

  onInputBioBlur() {
    this.inputBioFocused = false;
  }
  updateBioCharacterCount() {
    this.charCountBio = this.data.bio.length;
  }

  onInputLocationFocus() {
    this.inputLocationFocused = true;
  }

  onInputLocationBlur() {
    this.inputLocationFocused = false;
  }
  updateLocationCharacterCount() {
    this.charCountLocation = this.data.location.length;
  }


  getFirstInitial() {
    return this.data.currentUser.username[0].toUpperCase();
  }

  getAvatarColor() {
    return PostFormatter.getCircleColor(this.data.currentUser.username);
  }
}
