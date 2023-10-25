using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.ProfilesForAutoMapper;

public class PostCreateDtoToUserResolver : IValueResolver<PostCreateDTO, Post, User>
{
    private readonly UserQueryRepository _userQueryRepository;

    private readonly IMapper _mapper;

    public PostCreateDtoToUserResolver(IMapper mapper, UserQueryRepository userQueryRepository)
    {
        _mapper = mapper;
        _userQueryRepository = userQueryRepository;
    }

    public User Resolve(PostCreateDTO source, Post destination, User destMember, ResolutionContext context)
    {
        var authorId = source.AuthorId;
        var userModel = _userQueryRepository.GetUserById(authorId).Result;
        var user = _mapper.Map<User>(userModel);
        return user;
    }
}