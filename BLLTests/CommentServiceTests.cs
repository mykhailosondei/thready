using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationDAL.Context;
using AutoMapper;
using Moq;
using Xunit.Abstractions;

namespace BLLTests;

public class CommentServiceTests
{
    private readonly CommentService _commentService;
    private readonly Mock<PostService> _postServiceMock = new();
    private readonly Mock<UserService> _userServiceMock = new();
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ITestOutputHelper _outputHelper;
    
    public CommentServiceTests(ITestOutputHelper outputHelper)
    {
        _commentService = new CommentService(
        _applicationContextMock.Object,
        _mapperMock.Object,
        _postServiceMock.Object,
        _userServiceMock.Object
        );
        _outputHelper = outputHelper;
    }
    
}