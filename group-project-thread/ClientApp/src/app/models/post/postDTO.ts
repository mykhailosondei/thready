import {Image } from "../image";
import { UserDTO } from "../userDTO";

export interface PostDTO {
id : number;
createdAt: Date;
userId: number;
author: UserDTO;
images: Image[];
likesIds: number[];
commentsIds: number[];
repostersIds: number[];
bookmarks: number;
viwedBy: number[];
}
