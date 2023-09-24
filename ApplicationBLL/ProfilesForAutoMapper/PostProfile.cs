using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<PostDTO, Post>();
        CreateMap<Post, PostDTO>();
        CreateMap<PostCreateDTO, Post>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom<PostCreateDtoToUserResolver>());
    }
}