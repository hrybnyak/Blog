using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }

    }
}
