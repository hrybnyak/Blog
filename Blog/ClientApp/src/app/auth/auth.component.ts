import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { User } from '../models/user';
import { AuthResponse } from '../models/auth-response';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {
  mode: string = '';
  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  error: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.mode = params['mode'];
      this.checkMode();
    });
  }

  register() {
    let user = {
      UserName: this.name,
      Email: this.email,
      Password: this.password
    };
    this.registerUser(user);
  }

  private registerUser(user: { UserName: string, Email: string, Password: string }): any {
    this.authService.createAccount(user).subscribe((data: User) => {
      if (data !== null || data !== undefined) this.authService.login({ UserName: data.userName, Password: user.Password });
    },
    (error) => {
      this.error = "Sign up failed, please try to change email or name, because they might be already taken";
    });
  }


  login() {
    let user = {
      UserName: this.name,
      Password: this.password
    };
    let result = this.authService.login(user);
    if (typeof(result) !== typeof(AuthResponse))
    {
      this.error = "Authentication failed, please make sure your creditentials are correct.";
    }
  }

  private checkMode() {
    console.log(this.mode);
    if (!(this.mode === 'register' || this.mode === 'login')) {
      this.router.navigateByUrl(`/home`);
    }
  }
}
