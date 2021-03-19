using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModeratorApp.Models
{
    public class Comment
    {
        public int? Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int? ArticleId { get; set; }
        public string CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
