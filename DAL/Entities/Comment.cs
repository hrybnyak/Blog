using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    public class Comment : EntityBase
    {
        [Key]
        public override int Id { get; set; }
        public string Content { get; set; }
        public Article Article { get; set; }
        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
