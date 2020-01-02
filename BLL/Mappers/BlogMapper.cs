using BLL.DTO;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mappers
{
    public class BlogMapper : BaseMapper<BlogDTO, Blog>
    {
        public override BlogDTO Map(Blog element)
        {
            return new BlogDTO
            {
                Id = element.Id,
                Name = element.Name
            };
        }

        public override Blog Map(BlogDTO element)
        {
            return new Blog
            {
                Name = element.Name
            };
        }
    }
}
