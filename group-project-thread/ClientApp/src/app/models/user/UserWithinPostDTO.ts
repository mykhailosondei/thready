import {Image} from "../image";

export interface UserWithPostDTO{
  id: number;
  username: string;
  avatar: Image;
  bio: string;
  followers: number;
  following: number;
}
