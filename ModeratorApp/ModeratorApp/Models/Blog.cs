using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModeratorApp.Models
{
    public class Blog
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string OwnerUsername { get; set; }
    }
}
