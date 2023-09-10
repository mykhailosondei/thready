using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ForMember(dest => dest.Password, src => src.MapFrom(s => s.PasswordHash));
        CreateMap<RegisterUserDTO, User>().ForMember(dest => dest.Avatar, src => src.MapFrom(s => string.IsNullOrEmpty(s.Avatar) ? null : new Image { Url = s.Avatar }));
    }
}