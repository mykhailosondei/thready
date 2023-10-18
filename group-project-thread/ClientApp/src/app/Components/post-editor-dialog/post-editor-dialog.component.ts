import {Component, Inject, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CommentCreateDialogData} from "../../models/coment/CommentCreateDialogData";
import {PostEditDialogData} from "../../models/post/PostEditDialogData";
import {PageUserDTO} from "../../models/user/pageUserDTO";
import PostFormatter from "../../helpers/postFormatter";
import {faTimes} from "@fortawesome/free-solid-svg-icons";
import {animate, state, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-post-editor-dialog',
  templateUrl: './post-editor-dialog.component.html',
  styleUrls: ['./post-editor-dialog.component.scss', '../page-post/page-post.component.scss', '../comment-creation-dialog/comment-creation-dialog.component.scss'],
  animations: [
    trigger('fadeInOut', [
      state('void', style({
        opacity: 0,
      })),
      state('*', style({
        opacity: 1,
      })),
      transition('void <=> *', animate('100ms ease-in-out')),
    ]),
  ]
})
export class PostEditorDialogComponent {

  faTimes = faTimes;

  @ViewChild('editInputBox') editInputBox: {inputValue: string} = {inputValue: this.data.post.textContent}

  constructor(public dialogRef: MatDialogRef<PostEditorDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: PostEditDialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  isAvatarNull(user: PageUserDTO) {
    return user.avatar === null;
  }

  getCircleColor(user: PageUserDTO) {
    return PostFormatter.getCircleColor(user.username);
  }

  getFirstInitial(user: PageUserDTO) {
    return user.username[0].toUpperCase();
  }

  isButtonDisabled() {
    return PostFormatter.isInputLengthInvalid(this.editInputBox.inputValue) || this.editInputBox.inputValue === this.data.post.textContent;
  }
}
