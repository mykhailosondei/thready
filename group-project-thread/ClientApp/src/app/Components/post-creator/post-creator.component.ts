import {Component, ElementRef, ViewChild} from '@angular/core';
import {UserDTO} from "../../models/user/userDTO";
import {AuthService} from "../../Services/auth.service";
import {A} from "@angular/cdk/keycodes";
import {UserService} from "../../Services/user.service";
import PostFormatter from "../../helpers/postFormatter";
import {PostService} from "../../Services/post.service";
import {Image} from "../../models/image";

@Component({
  selector: 'app-post-creator',
  templateUrl: './post-creator.component.html',
  styleUrls: ['./post-creator.component.scss', '../page-post/page-post.component.scss']
})
export class PostCreatorComponent {

  currentUser= {} as UserDTO;
  @ViewChild('creatorInput') currentText: {inputValue: string} = {inputValue: ''};
  currentImages: string[] = [];

  //inject user service
  //inject post service
  constructor(private readonly userService: UserService, private readonly postService: PostService) { }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(Response => {
      this.currentUser = Response.body!;
    });
  }

  getFirstInitial(): string{
    return this.currentUser.username[0].toUpperCase();
  }

  getAvatarColor(): string{
    return PostFormatter.getCircleColor(this.currentUser.username);
  }

  isAvatarNull() {
    return this.currentUser.avatar == null;
  }

  createPost() {
    this.postService.createPost(
      {textContent: this.currentText.inputValue, images: this.currentImages.map<Image>(i => {return {url:i}})}
    ).subscribe(Response => {
      console.log(Response);
      if(Response.ok){
        this.currentText.inputValue = '';
        this.currentImages = [];
      }
    });
  }

  isButtonDisabled() : boolean {
    return PostFormatter.isInputLengthInvalid(this.currentText.inputValue);
  }
}
