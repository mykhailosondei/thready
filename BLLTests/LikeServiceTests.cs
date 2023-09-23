using System.Security.Cryptography.X509Certificates;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BLLTests;

public class LikeServiceTests
{
    private readonly LikeService _likeService;
    private readonly Mock<PostService> _postServiceMock = new();
    private readonly Mock<CommentService> _commentServiceMock = new();
    private readonly Mock<UserService> _userServiceMock = new();
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ITestOutputHelper _outputHelper;

    private List<Post> _posts = new List<Post>()
    {
        new Post()
        {
            Id = 1,
            TextContent = "post1",
            LikesIds = new List<int>() { 1, 2 }
        },
        new Post()
        {
            Id = 2,
            TextContent = "post2",
            LikesIds = new List<int>() { }
        },
        new Post()
        {
            Id = 3,
            TextContent = "post3",
            LikesIds = new List<int>(){1,2,3}
        }
    };
    
    private List<User> _users = new List<User>()
    {
        new User()
        {
            Id = 1,
            Username = "user1"
        },
        new User()
        {
            Id = 2,
            Username = "user2"
        },
        new User()
        {
            Id = 3,
            Username = "user3"
        }
    };

    public LikeServiceTests(ITestOutputHelper outputHelper)
    {
        _likeService = new LikeService(_applicationContextMock.Object,
            _mapperMock.Object,
            _postServiceMock.Object,
            _commentServiceMock.Object,
            _userServiceMock.Object);
        _outputHelper = outputHelper;
        
        _mapperMock.Setup(m => m.Map<PostDTO>(It.IsAny<Post>())).Returns((Post entity) =>
        {
            var postDto = new PostDTO
            {
                PostId = entity.Id,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images,
                LikesIds = entity.LikesIds,
                CommentsIds = entity.CommentsIds,
                RepostersIds = entity.RepostersIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            };
            return postDto;
        });
        
        _mapperMock.Setup(m => m.Map<Post>(It.IsAny<PostDTO>())).Returns((PostDTO entity) =>
            new Post
            {
                Id = entity.PostId,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images,
                LikesIds = entity.LikesIds,
                CommentsIds = entity.CommentsIds,
                RepostersIds = entity.RepostersIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            });
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
    public async Task LikePost_UpdatesPostOnValidLikeAndUser()
    {
        //Arrange
        int authorId = 1;
        int postId = 2;
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_posts);
        _applicationContextMock.Setup(c => c.Posts.Update(It.IsAny<Post>())).Callback((Post entity) =>
        {
            _posts.RemoveAll(p => p.Id == entity.Id);
            _posts.Add(entity);
        });
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_users);
        //Act
        await _likeService.LikePost(postId, authorId);
        //Assert
        
        Assert.Contains(_posts.FirstOrDefault(p => p.Id == postId).CommentsIds, id => id == authorId);
    }
}