import {Component, ElementRef, EventEmitter, Output, ViewChild} from '@angular/core';
import {UserDTO} from "../../models/user/userDTO";
import {AuthService} from "../../Services/auth.service";
import {A} from "@angular/cdk/keycodes";
import {UserService} from "../../Services/user.service";
import PostFormatter from "../../helpers/postFormatter";
import {PostService} from "../../Services/post.service";
import {Image} from "../../models/image";
import {ImageUploadService} from "../../Services/image-upload.service";
import {faTimes} from "@fortawesome/free-solid-svg-icons";
import {PostDTO} from "../../models/post/postDTO";

@Component({
  selector: 'app-post-creator',
  templateUrl: './post-creator.component.html',
  styleUrls: ['./post-creator.component.scss', '../page-post/page-post.component.scss']
})
export class PostCreatorComponent {

  @ViewChild('creatorInput') currentText: {inputValue: string} = {inputValue: ''};
  @Output() onPostCreated = new EventEmitter<PostDTO>();
  currentUser= {} as UserDTO;
  imageUrls: string[] = [];

  postingInProgress: boolean = false;

  //inject user service
  //inject post service
  constructor(private readonly userService: UserService,
              private readonly postService: PostService,
              private readonly imageUploadService: ImageUploadService
  ) { }

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
    if(this.postingInProgress) return;
    this.postingInProgress = true;
    this.postService.createPost(
      {textContent: this.currentText.inputValue, images: this.imageUrls.map(i => {return {url: i}})}).subscribe(Response => {
      console.log(Response);
      if(Response.ok){
        this.currentText.inputValue = '';
        this.imageUrls = [];
        this.onPostCreated.emit();
      }
    }).add(() => this.postingInProgress = false);
  }

  get isButtonDisabled() : boolean {
    return PostFormatter.isInputLengthTooBig(this.currentText.inputValue) && this.imageUrls.length === 0;
  }


  onPhotoLoaded($event: string) {
    this.imageUrls.push($event);
  }

  deleteImage($event: string) {
    this.imageUrls = this.imageUrls.filter(i => i !== $event);
    var deletionName = $event.split('/').pop()!;
    this.imageUploadService.deleteImage(deletionName).subscribe();
  }

  protected readonly faTimes = faTimes;
}
