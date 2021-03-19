using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModeratorApp.Clients;
using ModeratorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModeratorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IBlogHttpClient _blogClient;
        private User _adminSettings;

        public UsersController(IBlogHttpClient blogHttpClient, IOptions<User> adminSettings)
        {
            _blogClient = blogHttpClient;
            _adminSettings = adminSettings.Value;
            _blogClient.Authenticate(adminSettings.Value);
        }

        // GET api/values
        [HttpGet("count")]
        public async Task<ActionResult<IEnumerable<string>>> GetCount()
        {
            if (_blogClient.TokenExpired)
            {
                await _blogClient.Authenticate(_adminSettings);
            }
            var users = await _blogClient.GetUsers();
            var count = users.Count();
            return Ok(count);
        }

    }
}
