using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class CommentDTO
    {
        public int? Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int? ArticleId { get; set; }
        public string CreatorUsername { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}