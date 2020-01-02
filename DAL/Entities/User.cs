using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
