using ApplicationBLL.Exceptions;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
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

        _mapperMock.Setup(m => m.Map<CommentDTO>(It.IsAny<Comment>())).Returns((Comment entity) =>
            new CommentDTO()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PostId = entity.PostId,
                CommentId = entity.CommentId,
                TextContent = entity.TextContent,
                Images = entity.Images,
                CommentsIds = entity.CommentsIds,
                LikesIds = entity.LikesIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            }
        );
        _mapperMock.Setup(m => m.Map<Comment>(It.IsAny<CommentDTO>())).Returns((CommentDTO entity) =>
            new Comment()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PostId = entity.PostId,
                CommentId = entity.CommentId,
                TextContent = entity.TextContent,
                Images = entity.Images,
                CommentsIds = entity.CommentsIds,
                LikesIds = entity.LikesIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
                
            }
        );
    }
    
    [Fact]
    public async Task GetCommentById_ShouldThrowException_OnNonExistingId()
    {
        //Arrange
        int id = 2;
        var comments = new List<Comment>
        {
            new Comment()
            {
                Id = 1,
                TextContent = "konevych"
            }
        };
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        //Act
        //Assert
        var ex = await Assert.ThrowsAsync<CommentNotFoundException>(async () => await _commentService.GetCommentById(id));
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task GetCommentById_ShouldReturnValidComment_OnExistingId()
    {
        //Arrange
        int id = 1;
        var comments = new List<Comment>
        {
            new Comment()
            {
                Id = 1,
                TextContent = "konevych"
            }
        };
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        //Act
        var comment = await _commentService.GetCommentById(id);
        //Assert
        Assert.Equal(comment.TextContent, comments[0].TextContent);
    }
    
    
    
}