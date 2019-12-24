using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Blog: EntityBase
    {
        [Key]
        public override int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
