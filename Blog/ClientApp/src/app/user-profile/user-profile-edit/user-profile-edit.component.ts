import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/user';
import { Router } from '@angular/router';
import { ApplicationPaths } from 'src/app/app.constants';

@Component({
  selector: 'app-user-profile-edit',
  templateUrl: './user-profile-edit.component.html',
  styleUrls: ['./user-profile-edit.component.css']
})
export class UserProfileEditComponent implements OnInit {
  user: User = <User>{}
  error: string = ''
  
  constructor(private authService: AuthService, private userService: UserService, private router: Router) { }

  ngOnInit() {
    this.authService.getLoggedUser().subscribe((data: User) => {
      this.user = data;
    });
  }

  changeUserData() {
    console.log(this.user);
    this.userService.editUserInfo(this.authService.getId(), this.user).subscribe(() => this.error = '',
    err => this.error = "Couldn't update user data, try other email or username, these might be already taken.");
    this.router.navigateByUrl(ApplicationPaths.Profile);
  }
}
