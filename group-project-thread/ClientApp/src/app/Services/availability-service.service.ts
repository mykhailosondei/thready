import { Injectable } from '@angular/core';
import {RegisterUserDTO} from "../models/auth/registerUserDTO";
import {AuthUserDTO} from "../models/auth/authUserDTO";
import {HttpInternalService} from "./http-internal.service";

@Injectable({
  providedIn: 'root'
})
export class AvailabilityServiceService {

  constructor(private httpService: HttpInternalService) { }

  public isEmailAvailable(email: string){
    return this.httpService.getFullRequest<boolean>(`/api/User/isEmailAvailable?email=${email}`);
  }
}
