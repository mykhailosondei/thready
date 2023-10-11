import {Component, Inject} from '@angular/core';
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import PostFormatter from "../../helpers/postFormatter";
import {D} from "@angular/cdk/keycodes";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import { faTimes } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-comment-creation-dialog',
  templateUrl: './comment-creation-dialog.component.html',
  styleUrls: ['./comment-creation-dialog.component.scss', '../page-post/page-post.component.scss']
})
export class CommentCreationDialogComponent {
  faTimes = faTimes;
  constructor(public dialogRef: MatDialogRef<CommentCreationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: CommentCreateDialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  getCircleColor(user :UserWithPostDTO) {
    return  PostFormatter.getCircleColor(user.username);
  }

  isAvatarNull(user : UserWithPostDTO): boolean {
    return user.avatar === null;
  }



  getFirstInitial(user : UserWithPostDTO): string {
    return user.username[0].toUpperCase();
  }
  getCreatedDate() {
    return PostFormatter.getDateFormattedString(new Date(this.data.post.dateCreated));
  }

  isButtonDisabled() : boolean {
    return this.data.textContent === "" || this.data.textContent.length > 500;
  }
}
