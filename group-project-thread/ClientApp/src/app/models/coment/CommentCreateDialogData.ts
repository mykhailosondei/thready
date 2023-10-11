import {Image} from "../image";
import {PagePostDTO} from "../post/pagePostDTO";
import {UserWithPostDTO} from "../user/UserWithinPostDTO";

export interface CommentCreateDialogData {
  currentUser: UserWithPostDTO;
  post: PagePostDTO;
  textContent: string;
  images: Image[];
}
