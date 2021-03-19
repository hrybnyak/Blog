using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModeratorApp.Models
{
    public class AuthResponse
    {
        public string Id { get; set; }
        public string Auth_token { get; set; }
        public int Expires_in { get; set; }
    }
}
