import {UserDTO} from "../user/userDTO";

export interface AuthUserDTO{
    user: UserDTO;
    token: string;
}
