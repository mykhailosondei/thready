import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpInternalService {
  public baseUrl: string = environment.apiURL;
  public headers: HttpHeaders = new HttpHeaders();

  constructor(private http: HttpClient) { }

  public getHeaders() : HttpHeaders{
    return this.headers
  }

  public getHeader(header : string ) : string | null{
    return this.headers.get(header);
  }

  public setHeader(header: string, value : string): void {
    this.headers = this.headers.set(header, value);
  }
  public deleteHeader(key: string): void {
    this.headers.delete(key);
  }
  
  public getRequest<T>(url: string, httpParams?: HttpParams): Observable<T> {
    return this.http.get<T>(this.buildUrl(url), { headers: this.getHeaders(), params: httpParams });
  }

  public getFullRequest<T>(url: string, httpParams?: HttpParams): Observable<HttpResponse<T>> {
    return this.http.get<T>(this.buildUrl(url), { observe: 'response', headers: this.getHeaders(), params: httpParams });
  }

  public postRequest<T>(url: string, data: object): Observable<T> {
    return this.http.post<T>(this.buildUrl(url), data, { headers: this.getHeaders() });
  } 
  
  public postFullRequest<T>(url: string, data: object): Observable<HttpResponse<T>>{
    return this.http.post<T>(this.buildUrl(url), data, {observe: 'response', headers: this.getHeaders() });
  }

  public postClearRequest<T>(url: string, data: object): Observable<T>{
    return this.http.post<T>(this.buildUrl(url), data);
  }

  public putRequest<T>(url: string, data: object): Observable<T>{
    return this.http.put<T>(this.buildUrl(url), data, {headers: this.getHeaders()});
  }

  public putFullRequest<T>(url: string, data: object): Observable<HttpResponse<T>>{
    return this.http.put<T>(this.buildUrl(url), data, {headers: this.getHeaders(), observe : 'response'});
  }

  public deleteRequest<T>(url: string, httpParams?: any): Observable<T> {
    return this.http.delete<T>(this.buildUrl(url), { headers: this.getHeaders(), params: httpParams });
  }

  public deleteFullRequest<T>(url: string, httpParams?: any): Observable<HttpResponse<T>> {
    return this.http.delete<T>(this.buildUrl(url), { headers: this.getHeaders(), observe: 'response', params: httpParams });
  }

  public buildUrl(url: string): string {
    if (url.startsWith("https://") || url.startsWith("http://")){
      return url;
    }
    return this.baseUrl + url;
  }
  
  public convertDataToJSON(data: object) : string{
    return JSON.stringify(data);
  }
}

