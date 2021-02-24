import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { Post } from '../models/post';
import { tap, filter, map } from 'rxjs/operators';
import { ApiPaths, BaseUrl } from '../app.constants'
import { error } from 'protractor';

@Injectable({
  providedIn: 'root'
})

export class PostService {

  private url: string = BaseUrl + ApiPaths.Posts;

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getPostsWithTextFilter(textFilter : string):Observable<Post[]>
  {
    return this.http.get(this.url + ApiPaths.TextFilter + textFilter, this.httpOptions).pipe(
      tap((data:any) => console.dir(data)),
      filter((data:any)=> data && data !==null),
      map((data:any) => data));
  }

  getPostsWithTegFilter(tegFilter : string):Observable<Post[]>
  {
    return this.http.get(this.url + ApiPaths.TegFilter + tegFilter, this.httpOptions).pipe(
      tap((data:any) => console.dir(data)),
      filter((data:any)=> data && data !==null),
      map((data:any) => data));
  }
}
