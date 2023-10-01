using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.User;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class CommentCreateDTOToUserResolver : IValueResolver<CommentCreateDTO, CommentDTO, UserDTO>
{
    private readonly UserService _userService;

    public CommentCreateDTOToUserResolver(UserService userService)
    {
        _userService = userService;
    }

    public UserDTO Resolve(CommentCreateDTO source, CommentDTO destination, UserDTO destMember, ResolutionContext context)
    {
        return _userService.GetUserById(source.UserId).Result;
    }
}