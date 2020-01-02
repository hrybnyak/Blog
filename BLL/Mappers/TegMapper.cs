using BLL.DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mappers
{
    public class TegMapper : BaseMapper<Teg, TegDTO>
    {
        public override Teg Map(TegDTO element)
        {
            return new Teg
            {
                Id = element.Id,
                Name = element.Name
            };
        }

        public override TegDTO Map(Teg element)
        {
            return new TegDTO
            {
                Id = element.Id,
                Name = element.Name
            };
        }
    }
}
