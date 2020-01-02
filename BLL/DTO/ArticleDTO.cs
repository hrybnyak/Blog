using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class ArticleDTO
    {
        public int? Id { get; set; }

        [MaxLength(500)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime LastUpdate { get; set; }
        [Required]
        public int? BlogId { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<TegDTO> Tegs { get; set; }
    }
}