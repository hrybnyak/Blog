import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ApplicationPaths } from '../app.constants';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private authService: AuthService) { }
  paths:any = ApplicationPaths;
  ngOnInit() {
  }

}
