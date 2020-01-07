using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class BlogDTO
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string OwnerUsername { get; set; }
        public IEnumerable<ArticleDTO> Articles {get; set;}
    }
}
