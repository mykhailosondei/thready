using ApplicationDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.Like;

namespace ApplicationCommon.Interfaces
{
    public interface ILikeable
    {
        List<LikeDTO> Likes { get; set; }
    }
}
