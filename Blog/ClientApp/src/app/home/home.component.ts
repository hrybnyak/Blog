import { Component, OnInit, NgModule } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { User } from '../models/user';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

@NgModule({
  imports: [CommonModule]
})
export class HomeComponent implements OnInit {
  loggedUser: User;
  loading: Boolean = true;
  error: string = '';
  constructor(private authService: AuthService) {
   }

  getUser()
  {
    if(this.authService.isLogged()){
    this.authService.getLoggedUser().subscribe((data: User) => {
      this.loggedUser = data;
      this.loading = false;
      this.error = '';
    }, 
    err => this.error = "Coudn't get logged user")
    }
    else{
      this.loading = false;
    }
  }

  ngOnInit() {
    this.getUser();
  }

}
