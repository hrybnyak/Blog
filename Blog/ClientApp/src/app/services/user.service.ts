import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { BaseUrl, ApiPaths } from '../app.constants';
import { User } from '../models/user';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { Blog } from '../models/blog';
import { Password } from '../models/password';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  private url: string = BaseUrl + ApiPaths.Users;

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, private authService: AuthService) { }

  getUserById(id: string): Observable<User> {
    return this.http.get<User>(this.url + `/${id}`, this.httpOptions);
  }

  getUserBlogs(id: string): Observable<Blog[]> {
    return this.http.get<Blog[]>(this.url + `/${id}` + ApiPaths.Blogs, this.httpOptions);
  }
  getUserComments(id: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(this.url + `/${id}` + ApiPaths.Comments, this.httpOptions);
  }

  editUserInfo(id: string, item: User): Observable<any> {
    return this.http.put(this.url + `/${id}`, item, this.httpOptions);
  }

  deleteItem(id: string): Observable<any> {
    return this.http.delete(this.url + `/${id}`, this.httpOptions);
  }

  changePassword(id: string, item: Password): Observable<any> {
    return this.http.put(this.url + `/${id}/password`, item, this.httpOptions);
  }
}

