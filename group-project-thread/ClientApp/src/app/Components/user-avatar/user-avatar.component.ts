import {Component, Input} from '@angular/core';
import PostFormatter from "../../helpers/postFormatter";
import {UserDTO} from "../../models/user/userDTO";
import {PageUserDTO} from "../../models/user/pageUserDTO";

@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.scss']
})
export class UserAvatarComponent {

  @Input() public isParentView: boolean = false;
  @Input() public user : PageUserDTO | UserDTO;


  getFirstInitial(): string {
    return this.user.username[0].toUpperCase();
  }
  isAvatarNull(): boolean {
    return this.user.avatar === null || this.user.avatar.url === "";
  }

  getCircleColor() {
    return PostFormatter.getCircleColor(this.user.username);
  }
}
