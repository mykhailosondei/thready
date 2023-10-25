import {Image} from "../image";
import {PagePostDTO} from "../post/pagePostDTO";
import {PageUserDTO} from "../user/pageUserDTO";

export interface CommentCreateDialogData {
  currentUser: PageUserDTO;
  post: PagePostDTO;
  textContent: string;
  images: Image[];
}
