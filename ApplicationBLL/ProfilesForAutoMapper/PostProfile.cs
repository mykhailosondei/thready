using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<PostDTO, Post>().AfterMap((src, dest, context) =>
        {
            dest.Author = context.Mapper.Map<UserDTO, User>(src.Author);
        });
        CreateMap<Post, PostDTO>().AfterMap((src, dest, context) =>
        {
            dest.Author = context.Mapper.Map<User, UserDTO>(src.Author);
        });
        CreateMap<PostCreateDTO, Post>()
            .ForMember(dest => dest.Author, opt 
                => opt.MapFrom<PostCreateDtoToUserResolver>());
        
    }
}