import {Component, Inject, NgModule, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UpdateUserDialogData} from "../../models/user/updateUserDialogData";
import {faCamera, faXmark} from "@fortawesome/free-solid-svg-icons";
import {Image} from "../../models/image";


@Component({
  selector: 'app-user-update-dialog',
  templateUrl: './user-update-dialog.component.html',
  styleUrls: ['./user-update-dialog.component.scss']
})
export class UserUpdateDialogComponent implements OnInit{
  faCross = faXmark;
  bio: string = "";
  location: string = "";
  image: Image | null = null;
  faXmark = faXmark;
  faCamere = faCamera;
  constructor(public dialogRef: MatDialogRef<UserUpdateDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: UpdateUserDialogData) {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit(): void {
    this.bio = this.data.bio;
    this.location =this.data.location;
    this.image = this.data.avatar;
  }

  save(){

  }
  changeImage(){

  }

}
