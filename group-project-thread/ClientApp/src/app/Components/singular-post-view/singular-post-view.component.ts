import { Component } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UserWithPostDTO} from "../../models/user/UserWithinPostDTO";
import {UserService} from "../../Services/user.service";

@Component({
  selector: 'app-singular-post-view',
  templateUrl: './singular-post-view.component.html',
  styleUrls: ['./singular-post-view.component.scss']
})
export class SingularPostViewComponent {

    incomingUsername : string = "";
    incomingPostId : string = "";
    public author : UserWithPostDTO;
    constructor(private route : ActivatedRoute, private userService : UserService) {
      this.route.paramMap.subscribe(params => {
       this.incomingUsername = params.get('username') || "DefaultUsername";
      });
      this.route.paramMap.subscribe(params => {
        this.incomingPostId = params.get('id') || "DefaultPostId";
      });
    }
}
