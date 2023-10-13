import {Image} from "../image";
import {PostDTO} from "../post/postDTO";

export interface UserDTO {
  id: number;
  username: string;
  email: string;
  dateOfBirth: string;
  password: string;
  imageID: number | null;
  avatar: Image | null;
  posts: PostDTO[];
  postsCount: number;
  followersIds: number[];
  followingIds: number[];
  bio: string;
  location: string;
  bookmarkedPostsIds: number[];
  repostsIds: number;

}

