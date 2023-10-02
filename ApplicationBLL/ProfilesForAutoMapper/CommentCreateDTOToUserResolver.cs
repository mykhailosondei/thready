using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.User;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class CommentCreateDTOToUserResolver : IValueResolver<CommentCreateDTO, CommentDTO, UserDTO>
{
    private readonly UserQueryRepository _userQueryRepository;


    public CommentCreateDTOToUserResolver(UserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    public UserDTO Resolve(CommentCreateDTO source, CommentDTO destination, UserDTO destMember, ResolutionContext context)
    {
        return _userQueryRepository.GetUserById(source.UserId).Result;
    }
}