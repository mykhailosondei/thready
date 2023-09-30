import {UserDTO} from "../user/userDTO";

export interface AuthUserDTO{
    user: UserDTO;
    authToken: string;
}
