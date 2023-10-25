using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationBLL.Services.SearchLogic;
using ApplicationCommon.DTOs.Image;
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

public class PostServiceTests
{
    private readonly PostService _postService;
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new(MockBehavior.Strict);
    private readonly Mock<PostQueryRepository> _postQueryRepositoryMock = new();
    private readonly Mock<UserQueryRepository> _userQueryRepositoryMock = new();
    private readonly Mock<IValidator<PostDTO>> _postDTOValidatorMock = new();
    private readonly Mock<IValidator<PostUpdateDTO>> _postUpdateValidatorMock = new();
    private readonly Mock<PostsContentsIndexer> _postsContentIndexer = new();
    private readonly ITestOutputHelper _outputHelper;
    

    public PostServiceTests(ITestOutputHelper output)
    {
        _postService = new PostService(_applicationContextMock.Object, _mapperMock.Object,
            _postDTOValidatorMock.Object, _postQueryRepositoryMock.Object,
            _userQueryRepositoryMock.Object, _postUpdateValidatorMock.Object,
            _postsContentIndexer.Object
            );
        _outputHelper = output;
        
        _mapperMock.Setup(m => m.Map<ImageDTO>(It.IsAny<Image>())).Returns((Image image) =>
        {
            ImageDTO imageDTO = new ImageDTO()
            {
                Id = image.Id,
                Url = image.Url
            };
            return imageDTO;
        });
        
        _mapperMock.Setup(m => m.Map<Image>(It.IsAny<ImageDTO>())).Returns((ImageDTO imageDTO) =>
        {
            Image image = new Image()
            {
                Id = imageDTO.Id,
                Url = imageDTO.Url
            };
            return image;
        });

        _mapperMock.Setup(m => m.Map<PostDTO>(It.IsAny<Post>())).Returns((Post entity) =>
        {
            var postDto = new PostDTO
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images.Select(i => _mapperMock.Object.Map<ImageDTO>(i)).ToList(),
                LikesIds = entity.LikesIds,
                CommentsIds = entity.CommentsIds,
                RepostersIds = entity.RepostersIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            };
            return postDto;
        });

        _mapperMock.Setup(m => m.Map<Post>(It.IsAny<PostDTO>())).Returns((PostDTO postDTO) =>
            new Post
            {
                Id = postDTO.Id,
                CreatedAt = postDTO.CreatedAt,
                UserId = postDTO.UserId,
                Author = null,
                TextContent = postDTO.TextContent,
                Images = postDTO.Images.Select(i => _mapperMock.Object.Map<Image>(i)).ToList(),
                LikesIds = postDTO.LikesIds,
                CommentsIds = postDTO.CommentsIds,
                RepostersIds = postDTO.RepostersIds,
                Bookmarks = postDTO.Bookmarks,
                ViewedBy = postDTO.ViewedBy
            });

        _mapperMock.Setup(m => m.Map<Post>(It.IsAny<PostCreateDTO>()))
            .Returns((PostCreateDTO postCreateDTO) => new Post
            {
                UserId = postCreateDTO.AuthorId,
                TextContent = postCreateDTO.TextContent,
                Images = postCreateDTO.Images.Select(i => _mapperMock.Object.Map<Image>(i)).ToList(),
                CommentsIds = null
                
            });

        _mapperMock.Setup(m => m.Map<IEnumerable<PostDTO>>(It.IsAny<IEnumerable<Post>>()))
            .Returns((IEnumerable<Post> posts) => posts.Select(entity => new PostDTO
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                Author = null,
                TextContent = entity.TextContent,
                Images = entity.Images.Select(i => _mapperMock.Object.Map<ImageDTO>(i)).ToList(),
                LikesIds = entity.LikesIds,
                CommentsIds = entity.CommentsIds,
                RepostersIds = entity.RepostersIds,
                Bookmarks = entity.Bookmarks,
                ViewedBy = entity.ViewedBy
            }));
        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns((User entity) =>
            new UserDTO() { Id = entity.Id,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                Password = entity.PasswordHash,
                Username = entity.Username,
                ImageId = entity.ImageId,
                Avatar = _mapperMock.Object.Map<ImageDTO>(entity.Avatar),
                FollowersIds = entity.FollowersIds,
                FollowingIds = entity.FollowingIds,
                Bio = entity.Bio,
                Location = entity.Location,
                BookmarkedPostsIds = entity.BookmarkedPostsIds,
                RepostsIds = entity.RepostsIds
            });
        _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserDTO>())).Returns((UserDTO entity) =>
            new User() { Id = entity.Id,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                PasswordHash = entity.Password,
                Username = entity.Username,
                ImageId = entity.ImageId,
                Avatar = _mapperMock.Object.Map<Image>(entity.Avatar),
                FollowersIds = entity.FollowersIds,
                FollowingIds = entity.FollowingIds,
                Bio = entity.Bio,
                Location = entity.Location,
                BookmarkedPostsIds = entity.BookmarkedPostsIds,
                RepostsIds = entity.RepostsIds
            });
        
        _testUsers = new List<User>()
        {
            new User()
            {
                Id = 10,
                Posts = new List<Post>()
                {
                    new Post
                    {
                        Id = 1,
                        TextContent = "test post1"
                    },
                    new Post
                    {
                        Id = 2,
                        TextContent = "test post2"
                    }
                },
                RepostsIds = new List<int>(),
                BookmarkedPostsIds = new List<int>()
            },
            new User()
            {
                Id = 11,
                Username = "testuser11",
                Email = "test11@gmail.com",
                Posts = new List<Post>()
                {
                    new Post
                    {
                        Id = 3,
                        UserId = 11,
                        TextContent = "test post 3",
                        Bookmarks = 2,
                        
                    }
                },
                RepostsIds = new List<int>() {1},
                BookmarkedPostsIds = new List<int>()
            },
            
            new User()
            {
                Id = 12,
                Username = "testuser12",
                Email = "test12@gmail.com",
                Posts = new List<Post>()
                {
                    new Post()
                    {
                        Id = 4,
                        UserId = 12,
                        TextContent = "test post 4",
                        Bookmarks = 4,
                        RepostersIds = new List<int>() {10, 11}
                    }, 
                    new Post()
                    {
                    Id = 5,
                    UserId = 12,
                    TextContent = "test post 5",
                    Bookmarks = 0,
                    RepostersIds = new List<int>()
                }
                },
                BookmarkedPostsIds = new List<int>() {3},
                RepostsIds = new List<int>()
            }
        };

        _testUsersModels = new List<UserDTO>()
        {
            new UserDTO()
            {
                Id = 10,
                Posts = new List<PostDTO>()
                {
                    new PostDTO()
                    {
                        Id = 1,
                        UserId = 10,
                        TextContent = "test post1"
                    },
                    new PostDTO()
                    {
                        Id = 2,
                        UserId = 10,
                        TextContent = "test post2"
                    }
                }
            },
            new UserDTO()
            {
                Id = 11,
                Username = "testuser11",
                Email = "test11@gmail.com",
                Posts = new List<PostDTO>()
                {
                    new PostDTO()
                    {
                        Id = 3,
                        UserId = 11,
                        TextContent = "test post 3",
                        Bookmarks = 2,
                    }
                },
                RepostsIds = new List<int>() {1}
            },
            
            new UserDTO()
            {
                Id = 12,
                Username = "testuser12",
                Email = "test12@gmail.com",
                Posts = new List<PostDTO>()
                {
                    new PostDTO()
                    {
                        Id = 4,
                        UserId = 12,
                        TextContent = "test post 4",
                        Bookmarks = 4,
                        RepostersIds = new List<int>() {10, 11}
                    }, 
                    new PostDTO()
                    {
                        Id = 5,
                        UserId = 12,
                        TextContent = "test post 5",
                        Bookmarks = 0,
                        RepostersIds = new List<int>()
                    }
                },
                BookmarkedPostsIds = new List<int>() {3},
                RepostsIds = new List<int>()
            }
        };
        _mockPosts = new List<Post>()
        {
            new Post
            {
                Id = 1,
                TextContent = "test post1"
            },
            new Post
            {
                Id = 2,
                TextContent = "test post2"
            },
            new Post
            {
                Id = 3,
                UserId = 11,
                TextContent = "test post 3",
                Bookmarks = 2,
                        
            },
            new Post()
            {
                Id = 4,
                UserId = 12,
                TextContent = "test post 4",
                Bookmarks = 4,
                RepostersIds = new List<int>() { 10, 11 }
            },
            new Post()
            {
                Id = 5,
                UserId = 12,
                TextContent = "test post 5",
                Bookmarks = 0,
                RepostersIds = new List<int>()
            }

        };

        _mockPostsModels = new List<PostDTO>()
        {
            new PostDTO()
            {
                Id = 1,
                TextContent = "test post1"
            },
            new PostDTO()
            {
                Id = 2,
                TextContent = "test post2"
            },
            new PostDTO()
            {
                Id = 3,
                UserId = 11,
                TextContent = "test post 3",
                Bookmarks = 2,
                        
            },
            new PostDTO()
            {
                Id = 4,
                UserId = 12,
                TextContent = "test post 4",
                Bookmarks = 4,
                RepostersIds = new List<int>() { 10, 11 }
            },
            new PostDTO()
            {
                Id = 5,
                UserId = 12,
                TextContent = "test post 5",
                Bookmarks = 0,
                RepostersIds = new List<int>()
            }
        };
    }
    
    private List<User> _testUsers;
    private List<UserDTO> _testUsersModels;
    private List<Post> _mockPosts;
    private List<PostDTO> _mockPostsModels;
    
    

    [Fact]
    public async Task BookmarkPost_ShouldAddPostToUserBookmarks()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[2]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_mockPosts);
        int expectedBookmarkedIdLength = 2;
        int expectedPostId = 2;
        //Act
        await _postService.BookmarkPost(2, 12);
        //Assert
        Assert.Equal(expectedBookmarkedIdLength ,_testUsersModels[2].BookmarkedPostsIds.Count);
        Assert.Equal(expectedPostId, _testUsersModels[2].BookmarkedPostsIds[1]);
    }
    
    
    [Fact]
    public async Task BookmarkPost_ShouldDoNothingIfPostIdIsInUserBookmarksIds()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[2]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        int expectedBookmarkedIdLength = 1;
        //Act
        await _postService.BookmarkPost(3, 12);
        //Assert
        Assert.Equal(expectedBookmarkedIdLength ,_testUsersModels[2].BookmarkedPostsIds.Count);
    }
    
    [Fact]
    public async Task RemoveFromBookmarksPost_ShouldRemovePostFromUserBookmarks()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[2]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        
        int expectedBookmarkedIdLength = 0;
        //Act
        await _postService.RemoveFromBookmarksPost(3, 12);
        //Assert
        Assert.Equal(expectedBookmarkedIdLength ,_testUsersModels[2].BookmarkedPostsIds.Count);
    }
    
    [Fact]
    public async Task RemoveFromBookmarksPost_ShouldDoNothingIfPostIdIsNotInUserBookmarksIds()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[2]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        int expectedBookmarkedIdLength = 1;
        //Act
        await _postService.RemoveFromBookmarksPost(5, 12);
        //Assert
        Assert.Equal(expectedBookmarkedIdLength ,_testUsersModels[2].BookmarkedPostsIds.Count);
    }
    
    [Fact]
    public async Task Repost_ShouldAddPostIdToUserRepostIds()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[1]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        int expectedRepostIdLength = 2;
        int expectedPostId = 5;
        //Act
        await _postService.Repost(5, 11);
        //Assert
        Assert.Equal(expectedRepostIdLength ,_testUsersModels[2].RepostsIds.Count);
        Assert.Equal(expectedPostId, _testUsersModels[2].RepostsIds[1]);
    }
    
    [Fact]
    public async Task Repost_ShouldDoNothingIfPostAlreadyInUserReposts()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[1]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        int expectedRepostIdLength = 1;
        int expectedPostId = 1;
        //Act
        await _postService.Repost(1, 11);
        //Assert
        Assert.Equal(expectedRepostIdLength ,_testUsersModels[1].RepostsIds.Count);
        Assert.Equal(expectedPostId, _testUsersModels[1].RepostsIds[0]);
    }
    
    [Fact]
    public async Task UndoRepost_ShouldRemovePostIdFromUserRepostIds()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[1]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        int expectedRepostIdLength = 0;
        //Act
        await _postService.UndoRepost(1, 11);
        //Assert
        Assert.Equal(expectedRepostIdLength ,_testUsersModels[2].RepostsIds.Count);
    }
    
    [Fact]
    public async Task UndoRepost_ShouldDoNothingIfPostIsNotInUserReposts()
    {
        
        //Arrange
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(_testUsers);
        _userQueryRepositoryMock.Setup(c => c.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsersModels[1]);
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(this._mockPosts);
        
        int expectedRepostIdLength = 1;
        int expectedPostId = 1;
        //Act
        await _postService.UndoRepost(5, 11);
        //Assert
        Assert.Equal(expectedRepostIdLength ,_testUsersModels[1].RepostsIds.Count);
        Assert.Equal(expectedPostId, _testUsersModels[1].RepostsIds[0]);
    }

    [Fact]
    public async Task CreatePost_ShouldCreateValidPost()
    {
        //Arrange
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_mockPosts);

        _postDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<PostDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>()
            });
        
        var createdPost = new PostCreateDTO
        {
            AuthorId = 1,
            TextContent = "test content",
            Images = new List<ImageDTO>()
        };

        var expectedPostEntity = new Post()
        {
            TextContent = createdPost.TextContent
        };
        _applicationContextMock.Setup(c => c.Posts.Add(It.IsAny<Post>()))
            .Callback<Post>(postEntity =>
            {
                expectedPostEntity.Id = 6;
                _mockPosts.Add(expectedPostEntity);
            });
        
        int expectedPostId = 6;
        int expectedPostsCount = 6;
        //Act
        await _postService.CreatePost(createdPost);
        //Assert
        Assert.Equal(expectedPostId, _mockPosts[5].Id);
        Assert.Equal(expectedPostsCount, _mockPosts.Count);

    }
    
    [Fact]
    public async Task CreatePost_ShouldThrowExceptionOnInvalidPost()
    {
        //Arrange
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_mockPosts);
        _postDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<PostDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure()}
            });
        
        var createdPost = new PostCreateDTO
        {
            AuthorId = 1,
            TextContent = "",
            Images = new List<ImageDTO>()
        };
        
        var expectedPostEntity = new Post()
        {
            TextContent = createdPost.TextContent,
            Images = createdPost.Images.Select(i => _mapperMock.Object.Map<Image>(i)).ToList()
        };
        

        int expectedPostsCount = 5;
        //Assert & act
        var ex = await Assert.ThrowsAsync<EmptyPostException>(
            async () => await _postService.CreatePost(createdPost)
        );
        _outputHelper.WriteLine("" + ex);
        Assert.Equal(expectedPostsCount, _mockPosts.Count);

    }

    [Fact]
    public async Task PutPost_ShouldUpdatePost_WhenValidInputIsProvided()
    {
        //Arrange
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_mockPosts);
        _postDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<PostDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>()
            });
        int postToUpdateID = 1;
        PostUpdateDTO updatedPost = new PostUpdateDTO
        {
            
            TextContent = "update text",
            Images = new List<ImageDTO>()
            {
                new ImageDTO()
                {
                    Id = 1,
                    Url = "updatedImage.jpg"
                }
            }

        };

        _applicationContextMock.Setup(c => c.Posts.Update(It.IsAny<Post>())).
            Callback<Post>((entity) =>
            {
                entity.Id = postToUpdateID;
                _mockPosts.Remove(_mockPosts[0]);
                _mockPosts.Add(entity);
            });
        //Act
        await _postService.PutPost(postToUpdateID, updatedPost);
        
        //Assert
        Assert.Equal(updatedPost.TextContent, _mockPosts[4].TextContent);
        Assert.Equal(updatedPost.Images.Count, _mockPosts[4].Images.Count);

    }
    
    [Fact]
    public async Task PutPost_ShouldThrowAnExceptionWhenInvalidInfoProvided()
    {
        //Arrange
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_mockPosts);
        _postDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<PostDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure()}
            });
        
        int postToUpdateID = 1;
        PostUpdateDTO updatedPost = new PostUpdateDTO()
        {
            
            TextContent = "",
            Images = new List<ImageDTO>()

        };
        string actualText = _mockPosts[0].TextContent;
        //Assert & act
        var ex = await Assert.ThrowsAsync<EmptyPostException>(
            async () => await _postService.PutPost(postToUpdateID, updatedPost)
        );
        _outputHelper.WriteLine("" + ex);
        Assert.Equal(actualText, _mockPosts[0].TextContent);

    }

    [Fact]
    public async Task DeletePost_ShouldSuccessfullyDeletePost()
    {
        //Arrange
        int postId = 1;
        var actualPost = _mockPosts[0];
        _applicationContextMock.Setup(c => c.Posts).ReturnsDbSet(_mockPosts);
        _applicationContextMock.Setup(c => c.Posts.Remove(It.IsAny<Post>()))
            .Callback<Post>(( postEntity) =>
            {
                // Simulate the removal of the user from the database
                _mockPosts.Remove(actualPost);
            });
        //Act
        await _postService.DeletePost(postId);
        //Assert
        Assert.DoesNotContain(actualPost, _mockPosts);
    }
    
    
}