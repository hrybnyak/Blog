using BLL.DTO;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mappers
{
    public class CommentMapper : BaseMapper<Comment, CommentDTO>
    {
        public override Comment Map(CommentDTO element)
        {
            return new Comment
            {
                Content = element.Content,
                ArticleId = element.ArticleId.GetValueOrDefault()
            };
        }

        public override CommentDTO Map(Comment element)
        {
            return new CommentDTO
            {
                Id = element.Id,
                Content = element.Content,
                LastUpdate = element.LastUpdated,
                ArticleId = element.ArticleId
            };
        }
    }
}
