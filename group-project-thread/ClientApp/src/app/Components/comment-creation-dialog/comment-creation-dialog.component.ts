import {Component, Inject} from '@angular/core';
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-comment-creation-dialog',
  templateUrl: './comment-creation-dialog.component.html',
  styleUrls: ['./comment-creation-dialog.component.scss']
})
export class CommentCreationDialogComponent {

  constructor(public dialogRef: MatDialogRef<CommentCreationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: CommentCreateDialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
