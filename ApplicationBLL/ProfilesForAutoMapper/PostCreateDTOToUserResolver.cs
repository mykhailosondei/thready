using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class PostCreateDtoToUserResolver : IValueResolver<PostCreateDTO, Post, User>
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public PostCreateDtoToUserResolver(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public User Resolve(PostCreateDTO source, Post destination, User destMember, ResolutionContext context)
    {
        var authorId = source.AuthorId;
        var userModel = _userService.GetUserById(authorId);
        var user = _mapper.Map<User>(userModel);
        return user;
    }
}