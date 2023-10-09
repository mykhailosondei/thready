import {Image} from "../image";

export interface UserAccountDTO {
  id: number;
  username: string;
  avatar: Image | null;
  bio: string;
  location: string;
  followers: number;
  following: number;
  posts: number;
}
