using BLL.DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mappers
{
    public class ArticleMapper : BaseMapper<Article, ArticleDTO>
    {
        public override Article Map(ArticleDTO element)
        {
            return new Article
            {
                Id = element.Id,
                Name = element.Name,
                Content = element.Content,
                BlogId = element.BlogId
            };
        }

        public override ArticleDTO Map(Article element)
        {
            return new ArticleDTO
            {
                Id = element.Id,
                Name = element.Name,
                Content = element.Content,
                BlogId = element.BlogId,
                Created = element.Created
            };
        }
    }
}
