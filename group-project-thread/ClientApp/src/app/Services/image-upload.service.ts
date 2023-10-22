import { Injectable } from '@angular/core';
import {HttpInternalService} from "./http-internal.service";
import {Observable} from "rxjs";
import {HttpResponse} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {
  constructor(private httpService:HttpInternalService) { }

  public uploadImage(image: File): Observable<HttpResponse<{url:string}>> {
    const formData = new FormData();
    formData.append('file', image);
    return this.httpService.postFullRequest(`/api/ImageUpload/upload`, formData);
  }

  public deleteImage(imageName: string): Observable<HttpResponse<any>> {
    return this.httpService.deleteFullRequest(`/api/ImageUpload/delete?fileName=${imageName}`);
  }

}
