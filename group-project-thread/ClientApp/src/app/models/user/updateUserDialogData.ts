import {Image} from "../image";
import {UserDTO} from "./userDTO";

export interface UpdateUserDialogData{
  currentUser: UserDTO;
  bio: string;
  location: string
  avatar: Image | null;
}
