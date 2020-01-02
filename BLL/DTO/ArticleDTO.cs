using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public int BlogId { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<TegDTO> Tegs { get; set; }
    }
}