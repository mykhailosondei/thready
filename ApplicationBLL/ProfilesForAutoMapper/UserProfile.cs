using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ForMember(dest => dest.Password, src => src.MapFrom(s => s.PasswordHash)).AfterMap((src, dest, context) =>
        {
            dest.Avatar = context.Mapper.Map<Image, ImageDTO>(src.Avatar);
        });;
        CreateMap<UserDTO, User>().ForMember(dest => dest.PasswordHash, src => src.MapFrom(s => s.Password)).AfterMap((src, dest, context) =>
        {
            dest.Avatar = context.Mapper.Map<ImageDTO, Image>(src.Avatar);
        });
        CreateMap<RegisterUserDTO, User>().ForMember(dest => dest.Avatar, src => src.MapFrom(s => new Image { Url = string.IsNullOrEmpty(s.Avatar) ? "" : s.Avatar }));
        CreateMap<UserUpdateDTO, UserDTO>().ReverseMap();
        CreateMap<UserDTO, PageUserDTO>()
            .ForMember(dest => dest.Followers, src => src.MapFrom(s => s.FollowersIds.Count))
            .AfterMap((src, dest) => dest.Following = src.FollowingIds.Count);
        CreateMap<User, PageUserDTO>()
            .ForMember(dest => dest.Followers, src => src.MapFrom(s => s.FollowersIds.Count))
            .AfterMap(( src, dest, context) =>
            {
                dest.Following = src.FollowingIds.Count;
                dest.Avatar = context.Mapper.Map<Image, ImageDTO>(src.Avatar);
            });
        
    }
}