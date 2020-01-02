using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Article: EntityBase
    {
        [Key]
        public override int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime LastUpdate { get; set; }
        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public Blog Blog{ get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleTeg> ArticleTegs { get; set; }
     }
}
