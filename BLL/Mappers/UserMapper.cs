using BLL.DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mappers
{
    public class UserMapper : BaseMapper<User, UserDTO>
    {
        public override User Map(UserDTO element)
        {
            return new User
            {
                UserName = element.UserName,
                Email = element.Email
            };
        }

        public override UserDTO Map(User element)
        {
            return new UserDTO
            {
                UserName = element.UserName,
                Id = element.Id,
                Email = element.Email
            };
        }
    }
}
