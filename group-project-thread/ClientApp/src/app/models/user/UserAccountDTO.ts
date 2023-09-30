import {Image} from "../image";

export interface UserAccountDTO{
  id: number;
  username: string;
  avatar: Image;
  bio: string;
  location: string;
  followers: number;
  following: number;
  posts: number;
}
