import {Component, Inject} from '@angular/core';
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import PostFormatter from "../../helpers/postFormatter";
import {D} from "@angular/cdk/keycodes";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import {animate, state, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-comment-creation-dialog',
  templateUrl: './comment-creation-dialog.component.html',
  styleUrls: ['./comment-creation-dialog.component.scss', '../page-post/page-post.component.scss'],
  animations: [
    trigger('fadeInOut', [
      state('void', style({
        opacity: 0,
      })),
      state('*', style({
        opacity: 1,
      })),
      transition('void <=> *', animate('300ms ease-in-out')),
    ]),
  ]
})
export class CommentCreationDialogComponent {
  faTimes = faTimes;
  constructor(public dialogRef: MatDialogRef<CommentCreationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: CommentCreateDialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  getCircleColor(user :PageUserDTO) {
    return  PostFormatter.getCircleColor(user.username);
  }

  isAvatarNull(user : PageUserDTO): boolean {
    return user.avatar === null;
  }



  getFirstInitial(user : PageUserDTO): string {
    return user.username[0].toUpperCase();
  }
  getCreatedDate() {
    return PostFormatter.getDateFormattedElapsed(new Date(this.data.post.dateCreated));
  }

  isButtonDisabled() : boolean {
    return PostFormatter.isInputLengthInvalid(this.data.textContent);
  }
}
