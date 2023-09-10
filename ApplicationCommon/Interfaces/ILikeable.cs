using ApplicationCommon.DTOs.Like;
using ApplicationDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCommon.Interfaces
{
    public interface ILikeable
    {
        List<LikeDTO> Likes { get; set; }
    }
}
