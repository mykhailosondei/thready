import {Image} from "../image";
import {PageUserDTO} from "../user/pageUserDTO";

export interface PagePostDTO {
  id: number;
  textContent: string;
  imagesUrls: string[];
  user: PageUserDTO;
  commentsAmount: number;
  repostsAmount: number;
  likesAmount: number;
  viewsAmount: number;
  bookmarksAmount: number;
  dateCreated: string;
}
