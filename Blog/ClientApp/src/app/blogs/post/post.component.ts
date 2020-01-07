import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';

import { Post } from '../../models/post';
import { DataService } from '../../services/data.service';
import { Category } from '../../models/category';
import { ApiPaths, ApplicationPaths } from '../../app.constants';
import { stringify } from 'querystring';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
  post: Post = <Post>{};
  categoriesString: string = "";
  categories: Category[] = [];
  mode: string = 'create';
  id: any;
  blogId: number;


  constructor(private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router) {
    this.post.tegs = <Category[]>{};
  }

  ngOnInit() {
    this.blogId = <number>this.route.snapshot.params['blogId'];
    this.route.params.subscribe((params: Params) => {
      this.checkMode(params);
    });
  }

  editPost(): void {
    this.dataService.editItem<Post>(ApiPaths.Posts, this.id, this.post).subscribe(res => this.redirectToPostView());
  }

  createPost(): void {
    var data = {
      tegs: this.getCategoriesFromString(),
      name: this.post.name,
      content: this.post.content,
      blogId: +this.blogId
    };
    this.dataService.createItem<Post>(ApiPaths.Posts, data).subscribe(post => {
      this.id = post.id
      this.redirectToPostView();
    });
  }

  private redirectToPostView(): void {
    this.router.navigateByUrl(`${ApplicationPaths.PostView}/${this.id}`);
  }

  private getCategoriesFromString(): Category[] {
    console.log(this.categoriesString);
    let names: string[] = this.categoriesString.split(',');
    names.forEach(name => {
      let teg: Category = new Category();
      teg.name = name;
      this.categories.push(teg);
    });
    return this.categories;
  }

  private getCategoriesToString(): string {
    let result: string;
    for (let i = 0; i < this.categories.length; i++) {
      result += this.categories[i].name
      if (i !== this.categories.length - 1) {
        result += ','
      }
    }
    return result;
  }

  private checkMode(params: Params) {
    this.id = params['id'];

    if (this.id !== 'create') {
      this.mode = 'edit';
      this.id = +this.id;
      this.dataService.getItemById<Post>(ApiPaths.Posts, this.id).subscribe(post => this.post = post)
      this.categories = this.post.tegs;
      this.categoriesString = this.getCategoriesToString();
    } else {
      this.mode = 'create';
    }
  }
}
