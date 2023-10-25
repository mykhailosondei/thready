using ApplicationCommon.DTOs.Image;
using ApplicationDAL.Entities;
using AutoMapper;
namespace ApplicationBLL.ProfilesForAutoMapper;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>().ReverseMap();
    }
}