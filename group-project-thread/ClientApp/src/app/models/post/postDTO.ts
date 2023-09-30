import {Image } from "../image";
import {UserDTO} from "../user/userDTO";
export interface PostDTO {
id : number;
createdAt: string;
userId: number;
author: UserDTO;
textContent: string;
images: Image[];
likesIds: number[];
commentsIds: number[];
repostersIds: number[];
bookmarks: number;
viwedBy: number[];
}
