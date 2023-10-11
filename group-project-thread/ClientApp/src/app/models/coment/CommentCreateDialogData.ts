import {Image} from "../image";
import {PagePostDTO} from "../post/pagePostDTO";

export interface CommentCreateDialogData {
  post: PagePostDTO;
  textContent: string;
  images: Image[];
}
