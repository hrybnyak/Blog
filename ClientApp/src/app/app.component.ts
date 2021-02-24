import { Component, OnInit } from '@angular/core';
import { Post } from './models/post';
import { DataService } from './services/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Blog';

  constructor(private dataService: DataService) { }

  ngOnInit() {}
}
