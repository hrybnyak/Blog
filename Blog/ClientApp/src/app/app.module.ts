import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { PostsComponent } from './posts/posts.component';
import { PostListItemComponent } from './posts/post-list-item/post-list-item.component';
import { PostViewComponent } from './posts/post-view/post-view.component';
import { NavbarComponent } from './navbar/navbar.component';
import { PostComponent } from './blogs/post/post.component';
import { PaginationComponent } from './posts/pagination/pagination.component';
import { CommentListComponent } from './posts/post-view/comment/comment-list/comment-list.component';
import { AddCommentComponent } from './posts/post-view/comment/add-comment/add-comment.component';
import { AuthComponent } from './auth/auth.component';
import { CompareValidatorDirective } from './compare-validator.directive';
import { AuthInterceptor } from './interceptor/auth-interceptor';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { ProfilePictureComponent } from './user-profile/profile-picture/profile-picture.component';
import { UserProfileEditComponent } from './user-profile/user-profile-edit/user-profile-edit.component';
import { ChangePasswordComponent } from './user-profile/change-password/change-password.component';
import { BlogComponent } from './blogs/blog/blog.component';
import { BlogViewComponent } from './blogs/blog-view/blog-view.component';
import { BlogsComponent } from './blogs/blogs.component';
import { BlogListItemComponent } from './blogs/blog-list-item/blog-list-item.component'
import { UserProfileMenuComponent } from './user-profile/user-profile-menu/user-profile-menu.component';
import { CommonModule } from '@angular/common';
import { AuthenticatedUserGuard } from './guards/authenticated-user.guard';
import { NotAuthenticatedUserGuard } from './guards/not-authenticated-user.guard';
import { RegularUserGuard } from './guards/regular-user.guard';
import { ModeratorComponent } from './moderator/moderator.component';
import { UserListItemComponent } from './user-list-item/user-list-item.component';
import { ModeratorUserGuard } from './guards/moderator-user.guard';
import { AdminComponent } from './admin/admin.component';
import { ModeratorsListComponent } from './admin/moderators-list/moderators-list.component';
import { ModeratorsListItemComponent } from './admin/moderators-list-item/moderators-list-item.component';
import { AdminUserGuard } from './guards/admin-user.guard';
import { CreateModeratorComponent } from './admin/create-moderator/create-moderator.component';
import { ApplicationPaths } from './app.constants';

const routes: Routes = [
  { path: ApplicationPaths.Posts, component: PostsComponent, runGuardsAndResolvers: 'always' },
  { path: ApplicationPaths.Post, component: PostComponent, canActivate: [RegularUserGuard] },
  { path: ApplicationPaths.PostViewId, component: PostViewComponent },
  { path: ApplicationPaths.Blogs, component: BlogsComponent, canActivate: [RegularUserGuard] },
  { path: ApplicationPaths.Home, component: HomeComponent },
  { path: ApplicationPaths.Profile, component: UserProfileComponent, canActivate: [AuthenticatedUserGuard] },
  { path: ApplicationPaths.AuthMode, component: AuthComponent, canActivate: [NotAuthenticatedUserGuard] },
  { path: ApplicationPaths.Base, redirectTo: ApplicationPaths.Home, pathMatch: 'full' },
  { path: ApplicationPaths.Blog, component: BlogComponent, canActivate: [RegularUserGuard] },
  { path: ApplicationPaths.BlogViewId, component: BlogViewComponent, canActivate: [RegularUserGuard] },
  { path: ApplicationPaths.ModeratorUsers, component: ModeratorComponent, canActivate: [ModeratorUserGuard] },
  { path: ApplicationPaths.AdminUsers, component: AdminComponent, canActivate: [AdminUserGuard] },
  { path: ApplicationPaths.AdminModerators, component: ModeratorsListComponent, canActivate: [AdminUserGuard] },
  { path: ApplicationPaths.CreateModerator, component: CreateModeratorComponent, canActivate: [AdminUserGuard] }
];

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PostsComponent,
    PostListItemComponent,
    NavbarComponent,
    PostViewComponent,
    PostComponent,
    PaginationComponent,
    CommentListComponent,
    AddCommentComponent,
    AuthComponent,
    CompareValidatorDirective,
    UserProfileComponent,
    ProfilePictureComponent,
    UserProfileEditComponent,
    ChangePasswordComponent,
    BlogComponent,
    BlogsComponent,
    BlogViewComponent,
    BlogListItemComponent,
    UserProfileMenuComponent,
    ModeratorComponent,
    UserListItemComponent,
    AdminComponent,
    ModeratorsListComponent,
    ModeratorsListItemComponent,
    CreateModeratorComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' }),
    NgbModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
