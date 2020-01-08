import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { ApiPaths } from 'src/app/app.constants';

@Component({
  selector: 'app-create-moderator',
  templateUrl: './create-moderator.component.html',
  styleUrls: ['./create-moderator.component.css']
})
export class CreateModeratorComponent implements OnInit {
  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  error:string = '';

  constructor(private router: Router, private dataService: DataService) { }

  ngOnInit() {
  }

  register() {
    console.log('here');
    let user = {
      UserName: this.name,
      Email: this.email,
      Password: this.password
    };
    this.dataService.createItem(ApiPaths.AdminModerators, user).subscribe(()=>{
    this.router.navigateByUrl(ApiPaths.AdminModerators)
    },
    (error) => {
        this.error = "Couldn't create moderator account, please make sure the username and email haven't been taken already."
    });
  }

}
