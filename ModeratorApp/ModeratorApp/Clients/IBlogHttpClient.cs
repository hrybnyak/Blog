using ModeratorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModeratorApp.Clients
{
    public interface IBlogHttpClient
    {
        bool TokenExpired { get; }
        Task<IEnumerable<User>> GetUsers();
        Task Authenticate(User admin);
    }
}
