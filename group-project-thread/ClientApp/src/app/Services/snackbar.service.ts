import {Injectable} from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import {NotifierComponent} from "../Components/notifier/notifier.component";

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  constructor(private snackBar : MatSnackBar) {

  }

  public showErrorMessage(error : string) {
    this.snackBar.openFromComponent(NotifierComponent, {
      duration: 4000,
      data:{
        message : error
      },
      panelClass: "errorSnackBar"
    })
  }
}
