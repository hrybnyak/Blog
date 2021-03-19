export const BaseUrl = "http://http://172.31.31.16/api"

export const ApiPaths = {
    Posts: '/articles',
    Blogs: '/blogs',
    Comments: '/comments',
    Categories: '/tegs',
    Users: '/accounts',
    Auth: '/auth',
    ModeratorUsers: '/moderator/users',
    AdminUsers: '/admin/users',
    AdminModerators: '/admin/moderators',
    TextFilter: '/textFilter/',
    TegFilter: '/tegsFilter/'
}

let applicationPaths = {
    Base: '',
    Home: `home`,
    Blogs: `my-blogs`,
    Blog: `my-blogs/:id`,
    Profile: `user-profile`,
    AuthMode: `auth/:mode`,
    Login: `auth/login`,
    Register: `auth/register`,
    Posts: 'posts',
    Post: `my-blogs/:blogId/posts/:id`,
    PostView: 'posts/view',
    BlogView: 'my-blogs/view/',
    PostViewId: 'posts/view/:id',
    BlogViewId: 'my-blogs/view/:id',
    ModeratorUsers: 'moderator/users',
    AdminUsers: 'admin/users',
    AdminModerators: 'admin/moderators',
    CreateModerator: 'admin/moderators/create',
}

export const ApplicationPaths = applicationPaths;