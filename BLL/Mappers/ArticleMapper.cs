using BLL.DTO;
using DAL.Entities;

namespace BLL.Mappers
{
    public class ArticleMapper : BaseMapper<Article, ArticleDTO>
    {
        public override Article Map(ArticleDTO element)
        {
            return new Article
            {
                Name = element.Name,
                Content = element.Content,
                BlogId = element.BlogId.GetValueOrDefault()
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
                LastUpdate = element.LastUpdate
            };
        }
    }
}
