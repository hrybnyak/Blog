using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO
{
    public class AvatarDTO
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
