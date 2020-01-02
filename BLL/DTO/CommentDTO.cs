using System;

namespace BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public string CreatorUsername { get; set; }
        public DateTime Created { get; set; }
    }
}