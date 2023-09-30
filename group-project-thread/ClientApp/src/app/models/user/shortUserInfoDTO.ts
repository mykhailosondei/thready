export interface ShortUserInfoDTO{
  id: number;
  username: string;
  bio: string;
  followersIds: number[];
  followingIds: number[];
}
