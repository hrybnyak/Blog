import { InjectionToken, Injectable, Inject } from '@angular/core';
import * as jwt_decode from 'jwt-decode';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { filter, map, tap } from 'rxjs/operators'
import { User } from '../models/user';
import { AuthResponse } from '../models/auth-response';
import { ApiPaths, BaseUrl, ApplicationPaths } from '../app.constants'

export const TOKEN: string = 'jwt_token';
export const ID: string = 'id';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string = BaseUrl;

  private headers = new Headers({ 'Content-Type': 'application/json' });
  private httpOptions = {
    responseType: 'text',
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, private router: Router) {
  }

  getToken(): string {
    return localStorage.getItem(TOKEN);
  }

  getId(): string {
    return localStorage.getItem(ID);
  }

  getRole(): string {

    let decoded: string = jwt_decode(this.getToken());
    return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }

  isTokenExpired(token?: string): boolean {
    if (!token) token = this.getToken();
    if (!token) return true;

    const date = this.getTokenExpirationDate(token);
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  isLogged() {
    return localStorage.getItem(TOKEN) != null;
  }

  isAdmin() {
    if (this.isLogged()) {
      return this.getRole() === 'Admin';
    }
    return false;
  }

  isModerator() {
    if (this.isLogged()) {
      return this.getRole() === 'Moderator';
    }
    return false;
  }

  isRegularUser() {
    if (this.isLogged()) {
      return this.getRole() === 'RegularUser';
    }
    else return false;
  }

  login(user: { UserName: string, Password: string }): any {
    let result;
    this.authenticate(user).subscribe((data: AuthResponse) => {
      result = data;
      this.setToken(data.auth_token);
      this.setId(data.id)
      this.router.navigateByUrl(ApplicationPaths.Home);
    });
    return result;
  }

  logout(): void {
    localStorage.removeItem(TOKEN);
    this.router.navigateByUrl(ApplicationPaths.Home);
  }

  createAccount(user: any): Observable<User> {
    return this.http.post(this.baseUrl + ApiPaths.Users, user, {
      responseType: 'json',
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
    }).pipe(
      tap((data: any) => console.dir(data)),
      map((data: any) => data)
    );
  }


  authenticate(user: any): Observable<AuthResponse> {
    return this.http.post(this.baseUrl + ApiPaths.Auth, user, {
      responseType: 'json',
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      tap((data: any) => console.dir(data)),
      map((data: any) => data)
    );
  }

  getLoggedUser(): Observable<User> {
    return this.http.get(this.baseUrl + ApiPaths.Users + '/' + this.getId(), {
      responseType: 'json',
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    })
      .pipe(
        tap((data: any) => console.dir(data)),
        map((data: any) => data)
      )
  }
  private setToken(token: string): void {
    localStorage.setItem(TOKEN, token);
  }

  private setId(id: string): void {
    localStorage.setItem(ID, id);
  }

  private getTokenExpirationDate(token: string): Date {
    const decoded: any = jwt_decode(token);

    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }
}