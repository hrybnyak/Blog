import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';
import { Password } from 'src/app/models/password';
import { Router } from '@angular/router';
import { ApplicationPaths } from 'src/app/app.constants';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  password: Password = <Password>{};
  confirmPassword = '';

  constructor(private userService: UserService, private authService: AuthService, private router: Router) { }

  ngOnInit() { }

  changePassword() {
    this.userService.changePassword(this.authService.getId(), this.password).subscribe();
    this.router.navigateByUrl(ApplicationPaths.Profile);
  }

}
