import { Injectable } from '@angular/core';

import { Post } from '../models/post';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';
import { AuthService} from './auth.service';
import { BaseUrl } from '../app.constants'
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private url: string = BaseUrl;

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, private authService: AuthService) { }

  getItems<T>(path: string): Observable<T[]> {
    return this.http.get<T[]>(this.url + path, this.httpOptions);
  }

  getItemById<T>(path: string, id: number): Observable<T> {
    return this.http.get<T>(this.url + path + `/${id}`, this.httpOptions);
  }

  getBeloningsByItemId<T>(path: string, id: number, belonings:string): Observable<T> {
    return this.http.get<T>(this.url + path + `/${id}`+ belonings, this.httpOptions);
  }

  editItem<T>(path: string, id: number, item: any): Observable<any> {
    return this.http.put<T>(this.url+path+`/${id}`, item , this.httpOptions);
  }

  createItem<T>(path: string, item: any): Observable<T> {
    return this.http.post<T>(this.url + path, item, this.httpOptions);
  }

  deleteItem(path: string, id: number): Observable<any> {
    return this.http.delete(this.url + path + `/${id}`, this.httpOptions);
  }
}