using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationDAL.Context;
using AutoMapper;
using Moq;
using Xunit.Abstractions;

namespace BLLTests;

public class LikeServiceTests
{
    private readonly LikeService _likeService;
    private readonly Mock<PostService> _postServiceMock = new();
    private readonly Mock<CommentService> _commentServiceMock = new();
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ITestOutputHelper _outputHelper;

    public LikeServiceTests(ITestOutputHelper outputHelper)
    {
        _likeService = new LikeService(_applicationContextMock.Object,
            _mapperMock.Object,
            _postServiceMock.Object,
            _commentServiceMock.Object);
        _outputHelper = outputHelper;
    }
}