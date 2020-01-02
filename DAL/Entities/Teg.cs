using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Teg : EntityBase
    {
        [Key]
        public override int Id { get; set; }
        [MaxLength(120)]
        public string Name { get; set; }
        public ICollection<ArticleTeg> ArticleTegs { get; set; }
    }
}
