using ApplicationBLL.Exceptions;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
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
    private readonly Mock<IMapper> _mapperMock = new(MockBehavior.Strict);
    private readonly Mock<IValidator<CommentDTO>> _validatorMock = new();
    private readonly ITestOutputHelper _outputHelper;
    
    public CommentServiceTests(ITestOutputHelper outputHelper)
    {
        _commentService = new CommentService(
        _applicationContextMock.Object,
        _mapperMock.Object,
        _postServiceMock.Object,
        _userServiceMock.Object,
        _validatorMock.Object);
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
        
        _mapperMock.Setup(m => m.Map<CommentDTO>(It.IsAny<CommentCreateDTO>())).Returns((CommentCreateDTO entity) =>
            new CommentDTO()
            {
                UserId = entity.AuthorId,
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

        _mapperMock.Setup(m => m.Map<IEnumerable<PostDTO>>(It.IsAny<IEnumerable<Post>>()))
            .Returns((IEnumerable<Post> posts) => posts.Select(entity => new PostDTO
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
            }));
        
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

    [Fact]
    public async Task GetCommentsOfPostId_ShouldReturnAllComments_OnFullyValidInput()
    {
        
        //Arrange
        var posts = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                TextContent = "none",
                CommentsIds = new List<int>() { 1, 2 }
            },
            new Post()
            {
                Id = 2,
                TextContent = "none2",
                CommentsIds = new List<int>() { 3 }
            }
        };
        var comments = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                TextContent = "comm1"
            },
            new Comment()
            {
                Id = 2,
                TextContent = "comm2"
            },
            new Comment()
            {
                Id = 3,
                TextContent = "comm3"
            }
        };
        
        _postServiceMock.Setup(p => p.GetPostById(It.IsAny<int>())).ReturnsAsync((int id) => _mapperMock.Object.Map<PostDTO>(posts.FirstOrDefault(p => p.Id == id)));

        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        
        //Act
        var result = (await _commentService.GetCommentsOfPostId(1)).ToList();
        //Assert
        Assert.Equal(result[0].TextContent, comments[0].TextContent);
        Assert.Equal(result[1].TextContent, comments[1].TextContent);
    }
    
    [Fact]
    public async Task GetCommentsOfPostId_ShouldReturnComments_OnParticularyValidIds()
    {
        
        //Arrange
        var posts = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                TextContent = "none",
                CommentsIds = new List<int>() { 1, 4 }
            },
            new Post()
            {
                Id = 2,
                TextContent = "none2",
                CommentsIds = new List<int>() { 3 }
            }
        };
        var comments = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                TextContent = "comm1"
            },
            new Comment()
            {
                Id = 2,
                TextContent = "comm2"
            },
            new Comment()
            {
                Id = 3,
                TextContent = "comm3"
            }
        };
        
        _postServiceMock.Setup(p => p.GetPostById(It.IsAny<int>())).ReturnsAsync((int id) => _mapperMock.Object.Map<PostDTO>(posts.FirstOrDefault(p => p.Id == id)));

        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        
        //Act
        var result = (await _commentService.GetCommentsOfPostId(1)).ToList();
        //Assert
        Assert.Single(result);
        Assert.Equal(comments[0].TextContent, result[0].TextContent);
    }
    
    [Fact]
    public async Task GetCommentsOfPostId_ShouldReturnEmptyLis_OnInvalidIds()
    {
        
        //Arrange
        var posts = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                TextContent = "none",
                CommentsIds = new List<int>() { 4, 5 }
            },
            new Post()
            {
                Id = 2,
                TextContent = "none2",
                CommentsIds = new List<int>() { 3 }
            }
        };
        var comments = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                TextContent = "comm1"
            },
            new Comment()
            {
                Id = 2,
                TextContent = "comm2"
            },
            new Comment()
            {
                Id = 3,
                TextContent = "comm3"
            }
        };
        
        _postServiceMock.Setup(p => p.GetPostById(It.IsAny<int>())).ReturnsAsync((int id) => _mapperMock.Object.Map<PostDTO>(posts.FirstOrDefault(p => p.Id == id)));

        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        
        //Act
        var result = (await _commentService.GetCommentsOfPostId(1)).ToList();
        //Assert
        Assert.Empty(result);
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

        _userServiceMock.Setup(u => u.GetUserById(It.IsAny<int>())).ReturnsAsync((UserDTO)null);
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

        _userServiceMock.Setup(u => u.GetUserById(It.IsAny<int>())).ReturnsAsync((UserDTO)null);
        //Act
        
        //Assert

        var ex = Assert.ThrowsAsync<InvalidDataException>(async ()=> await _commentService.PostComment(comment));
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task PostComment_ShouldAddCommentToDbSet_OnValidInput()
    {
        //Arrange
        
        var comment = new CommentCreateDTO()
        {
            PostId = 1,
            TextContent = "NEW"
        };

        var posts = new List<Post>()
        {
            new()
            {
                Id = 1,
                CommentsIds = new List<int>() { 1, 2 }
            }
        };
        
        var comments = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                TextContent = "comm1"
            },
            new Comment()
            {
                Id = 2,
                TextContent = "comm2"
            },
            new Comment()
            {
                Id = 3,
                TextContent = "comm3"
            }
        };

        _userServiceMock.Setup(u => u.GetUserById(It.IsAny<int>())).ReturnsAsync((UserDTO)null);
        
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CommentDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>()
            });
        _applicationContextMock.Setup(c => c.Comments.Add(It.IsAny<Comment>())).Callback((Comment entity) =>
        {
            comments.Add(entity);
        });
        
        _postServiceMock.Setup(p => p.GetPostById(It.IsAny<int>())).ReturnsAsync((int id) => _mapperMock.Object.Map<PostDTO>(posts.FirstOrDefault(p => p.Id == id)));
        _postServiceMock.Setup(p => p.PutPost(It.IsAny<int>(), It.IsAny<PostDTO>())).Callback((int id, PostDTO dto) =>
        {
        });
        //Act
        await _commentService.PostComment(comment);
        //Assert
        Assert.Equal(true, comments.Any(c => c.TextContent == "NEW"));
    }
    
    [Fact]
    public async Task PostComment_ShouldAddCommentToDbSet_OnValidInput2()
    {
        //Arrange
        
        var comment = new CommentCreateDTO()
        {
            CommentId = 1,
            TextContent = "NEW"
        };
        
        var comments = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                TextContent = "comm1",
                CommentsIds = new List<int>() { 1, 2 }
            },
            new Comment()
            {
                Id = 2,
                TextContent = "comm2"
            },
            new Comment()
            {
                Id = 3,
                TextContent = "comm3"
            }
        };

        _userServiceMock.Setup(u => u.GetUserById(It.IsAny<int>())).ReturnsAsync((UserDTO)null);
        
        _applicationContextMock.Setup(c => c.Comments).ReturnsDbSet(comments);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CommentDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>()
            });
        _applicationContextMock.Setup(c => c.Comments.Add(It.IsAny<Comment>())).Callback((Comment entity) =>
        {
            comments.Add(entity);
        });
        
        //Act
        await _commentService.PostComment(comment);
        //Assert
        Assert.Equal(true, comments.Any(c => c.TextContent == "NEW"));
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
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CommentDTO>(), CancellationToken.None))
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
        var comment = new CommentDTO()
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
        var comment = new CommentDTO()
        {
            TextContent = "",
            Images = new List<Image>()
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
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CommentDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure()}
            });
        //Act
        //Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _commentService.PutComment(3, comment));
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task DeleteComment_ShouldDelete_OnValidInput()
    {
        //Arrange
        int idToDelete = 1;
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
        _applicationContextMock.Setup(c => c.Comments.Remove(It.IsAny<Comment>())).Callback((Comment entity) =>
        {
            comments.RemoveAll(c => c.Id == entity.Id);
        });
        
        //Act
        await _commentService.DeleteComment(idToDelete);
        //Assert
        Assert.Single(comments);
        Assert.DoesNotContain(comments, c => c.Id == idToDelete);
    }
}