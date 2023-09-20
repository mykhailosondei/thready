import { UserDTO } from "../userDTO";

export interface AuthUserDTO{
    user: UserDTO;
    authToken: string;
}