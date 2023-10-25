using System.Linq.Expressions;
using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BLLTests;

public class CommentServiceTests
{
    private readonly CommentService _commentService;
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<PostQueryRepository> _postQueryRepositoryMock = new();
    private readonly Mock<UserQueryRepository> _userQueryRepositoryMock = new();
    private readonly Mock<CommentQueryRepository> _commentQueryRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new(MockBehavior.Strict);
    private readonly Mock<IValidator<CommentDTO>> _commentValidatorMock = new();
    private readonly Mock<IValidator<CommentUpdateDTO>> _commentUpdateValidatorMock = new();
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<ILogger<CommentService>> _logger = new();
    
    public CommentServiceTests(ITestOutputHelper outputHelper)
    {
        _commentService = new CommentService(
        _applicationContextMock.Object,
        _mapperMock.Object,
        _commentValidatorMock.Object,
        _logger.Object,
        _postQueryRepositoryMock.Object,
        _userQueryRepositoryMock.Object,
        _commentQueryRepositoryMock.Object,
        _commentUpdateValidatorMock.Object
        );
        _outputHelper = outputHelper;

        _mapperMock.Setup(m => m.Map<ImageDTO>(It.IsAny<Image>())).Returns((Image image) =>
        {
            ImageDTO imageDTO = new ImageDTO()
            {
                Id = image.Id,
                Url = image.Url
            };
            return imageDTO;
        });
        
        _mapperMock.Setup(m => m.Map<Image>(It.IsAny<ImageDTO>())).Returns((ImageDTO imageEntity) =>
        {
            Image image = new Image()
            {
                Id = imageEntity.Id,
                Url = imageEntity.Url
            };
            return image;
        });
        
        _mapperMock.Setup(m => m.Map<CommentDTO>(It.IsAny<Comment>())).Returns((Comment entity) =>
            new CommentDTO()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PostId = entity.PostId,
                CommentId = entity.CommentId,
                TextContent = entity.TextContent,
                Images = entity.Images.Select(I => _mapperMock.Object.Map<ImageDTO>(I)).ToList(),
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
                Images = entity.Images.Select(i => _mapperMock.Object.Map<Image>(i)).ToList(),
                CommentsIds = entity.CommentsIds,
                LikesIds = entity.LikesIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
                
            }
        );
        
        _mapperMock.Setup(m => m.Map<CommentDTO>(It.IsAny<CommentCreateDTO>())).Returns((CommentCreateDTO entity) =>
            new CommentDTO()
            {
                UserId = entity.UserId,
                PostId = entity.PostId,
                CommentId = entity.CommentId,
                TextContent = entity.TextContent,
                Images = entity.Images
            }
        );
        
        _mapperMock.Setup(m => m.Map<PostDTO>(It.IsAny<Post>())).Returns((Post entity) =>
        {
            var postDto = new PostDTO
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images.Select(I => _mapperMock.Object.Map<ImageDTO>(I)).ToList(),
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
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images.Select(I => _mapperMock.Object.Map<Image>(I)).ToList(),
                LikesIds = entity.LikesIds,
                CommentsIds = entity.CommentsIds,
                RepostersIds = entity.RepostersIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            });

        _mapperMock.Setup(m => m.Map<IEnumerable<PostDTO>>(It.IsAny<IEnumerable<Post>>()))
            .Returns((IEnumerable<Post> posts) => posts.Select(entity => new PostDTO
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images.Select(I => _mapperMock.Object.Map<ImageDTO>(I)).ToList(),
                LikesIds = entity.LikesIds,
                CommentsIds = entity.CommentsIds,
                RepostersIds = entity.RepostersIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            }));
    }

    [Fact]
    public async Task PostComment_ShouldThrowDataExceptionOnInvalidComment()
    {
        //Arrange
        
        var comment = new CommentCreateDTO()
        {
            CommentId = 1,
            PostId = 1
        };

        _userQueryRepositoryMock.Setup(u => u.GetUserById(It.IsAny<int>())).ReturnsAsync((UserDTO)null);
        //Act
        
        //Assert

        var ex = Assert.ThrowsAsync<InvalidDataException>(async ()=> await _commentService.PostComment(comment));
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task PostComment_ShouldThrowDataExceptionOnInvalidComment2()
    {
        //Arrange
        
        var comment = new CommentCreateDTO()
        {
            CommentId = null,
            PostId = null
        };

        _userQueryRepositoryMock.Setup(u => u.GetUserById(It.IsAny<int>())).ReturnsAsync((UserDTO)null);
        //Act
        
        //Assert

        var ex = Assert.ThrowsAsync<InvalidDataException>(async ()=> await _commentService.PostComment(comment));
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task PostComment_ShouldThrowException_OnInvalidCommentInput()
    {
        //Arrange
        var comment = new CommentCreateDTO()
        {
            CommentId = 1,
            TextContent = "NEW"
        };
        var comments = new List<Comment>()
        {
            new()
            {
                Id = 1,
                TextContent = "bop"
            },
            new()
            {
                Id = 2,
                TextContent = "bop"
            }
        };
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        _commentValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CommentDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure()}
            });
        //Act
        //Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _commentService.PostComment(comment));
        _outputHelper.WriteLine("" + ex);
    }

    [Fact]
    public async Task PutComment_ShouldThrowException_OnInvalidCommentId()
    {
        //Arrange
        var comment = new CommentUpdateDTO()
        {
            TextContent = "NEW"
        };
        var comments = new List<Comment>()
        {
            new()
            {
                Id = 1,
                TextContent = "bop"
            },
            new()
            {
                Id = 2,
                TextContent = "bop"
            }
        };
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        //Act
        //Assert
        var ex = Assert.ThrowsAsync<CommentNotFoundException>(async () => await _commentService.PutComment(3, comment));
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task PutComment_ShouldThrowException_OnInvalidCommentInput()
    {
        //Arrange
        var comment = new CommentUpdateDTO()
        {
            TextContent = "",
            Images = new List<ImageDTO>()
        };
        var comments = new List<Comment>()
        {
            new()
            {
                Id = 1,
                TextContent = "bop"
            },
            new()
            {
                Id = 2,
                TextContent = "bop"
            }
        };
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        _commentValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CommentDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure()}
            });
        //Act
        //Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _commentService.PutComment(3, comment));
        _outputHelper.WriteLine("" + ex);
    }
}