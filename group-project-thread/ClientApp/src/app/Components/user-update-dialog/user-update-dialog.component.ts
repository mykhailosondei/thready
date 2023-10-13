import {Component, Inject, NgModule, OnInit, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UpdateUserDialogData} from "../../models/user/updateUserDialogData";
import {faCamera, faXmark} from "@fortawesome/free-solid-svg-icons";
import {Image} from "../../models/image";
import DevExpress from "devextreme/bundles/dx.all";
import data = DevExpress.data;


@Component({
  selector: 'app-user-update-dialog',
  templateUrl: './user-update-dialog.component.html',
  styleUrls: ['./user-update-dialog.component.scss']
})
export class UserUpdateDialogComponent implements OnInit{
  faCross = faXmark;
  image: Image | null = null;
  faXmark = faXmark;
  faCamere = faCamera;
  inputLocationFocused: boolean = false;
  inputBioFocused: boolean = false;
  charCountLocation: number = this.data.location.length;
  maxCharCountLocation: number = 30;
  charCountBio: number = this.data.bio.length;
  maxCharCountBio: number = 100;

  constructor(public dialogRef: MatDialogRef<UserUpdateDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: UpdateUserDialogData) {
  }
  ngOnInit(): void {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  changeImage(){

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


}
