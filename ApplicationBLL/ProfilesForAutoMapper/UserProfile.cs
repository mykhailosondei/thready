using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ForMember(dest => dest.Password, src => src.MapFrom(s => s.PasswordHash));
        CreateMap<UserDTO, User>().ForMember(dest => dest.PasswordHash, src => src.MapFrom(s => s.Password));
        CreateMap<RegisterUserDTO, User>().ForMember(dest => dest.Avatar, src => src.MapFrom(s => new Image { Url = string.IsNullOrEmpty(s.Avatar) ? "" : s.Avatar }));
    }
}