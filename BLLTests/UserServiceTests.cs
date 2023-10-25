using System.Net;
using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using group_project_thread.Validators;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BLLTests;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<UserQueryRepository> _userQueryRepositoryMock = new();
    private readonly Mock<PostQueryRepository> _postQueryRepositoryMock = new(); 
    private readonly Mock<CommentQueryRepository> _commentQueryRepositoryMock = new();
    private readonly Mock<EmailValidatorService> _emailValidatorServiceMock = new();
    private readonly Mock<IValidator<RegisterUserDTO>> _registerUserDTOValidatorMock = new();
    private readonly Mock<IValidator<UserUpdateDTO>> _userUpdateDTOValidatorMock = new();
    private readonly ITestOutputHelper _outputHelper;

    public UserServiceTests(ITestOutputHelper output)
    {
        _userService = new UserService(
            _applicationContextMock.Object,
            _mapperMock.Object,
            _emailValidatorServiceMock.Object,
            _registerUserDTOValidatorMock.Object,
            _userQueryRepositoryMock.Object,
            _postQueryRepositoryMock.Object,
            _userUpdateDTOValidatorMock.Object,
            _commentQueryRepositoryMock.Object
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
        
        _mapperMock.Setup(m => m.Map<Image>(It.IsAny<ImageDTO>())).Returns((ImageDTO imageEntity) =>
        {
            Image image = new Image()
            {
                Id = imageEntity.Id,
                Url = imageEntity.Url
            };
            return image;
        });
        
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
        _mapperMock.Setup(m => m.Map<User>(It.IsAny<RegisterUserDTO>())).Returns((RegisterUserDTO entity) =>
            new User()
            {
                Id = entity.UserId,
                Username = entity.Username,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth
            });
    }

    [Fact]
    public async Task CreateUser_ShouldReturnValidUserDTO_OnSuccess()
    {
        // Arrange
        var registerUserDto = new RegisterUserDTO
        {
            Email = "newuser@example.com",
            Password = "password",
            // Set other properties as needed
        };

        // Mock the required services
        _emailValidatorServiceMock.Setup(e => e.IsEmailAvailable(registerUserDto.Email))
            .ReturnsAsync(true);

        var expectedUserEntity = new User
        {
            Email = registerUserDto.Email
        };
        
        _registerUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {}
            });

        _applicationContextMock.Setup(c => c.Users.Add(It.IsAny<User>()))
            .Callback<User>(userEntity =>
            {
                // Simulate saving the user entity to the database
                expectedUserEntity.Id = 1; // Set the user's ID as if it were saved in the DB
            });
        

        // Act
        var userDto = await _userService.CreateUser(registerUserDto);

        // Assert
        Assert.NotNull(userDto);
        Assert.True(BCrypt.Net.BCrypt.Verify(registerUserDto.Password, userDto.Password));
        // Add more assertions as needed to validate the userDto
    }
    
    
    [Fact]
    public async Task CreateUser_ShouldThrowAnExceptionOnInvalidInput()
    {
        // Arrange
        var registerUserDto = new RegisterUserDTO
        {
            Email = "newuser@example.com",
            Password = "password",
            // Set other properties as needed
        };
        _registerUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure() {ErrorMessage = "date is not provided"}}
            });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(
            async () => await _userService.CreateUser(registerUserDto)
        );
        _outputHelper.WriteLine("" + ex);
        // Add more assertions as needed to validate the userDto
    }

    [Fact]
    public async Task Follow_ShouldThrowUserNotFoundException_OnInvalidUserId()
    {
        // Arrange
        int userIdToFollow = 1;
        int currentUserId = 2;

        // Mock GetUserById to return null, simulating a user not found scenario
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(new List<User>());

        // Act and Assert
        var ex = await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _userService.Follow(userIdToFollow, currentUserId)
        );
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task Unfollow_ShouldThrowUserNotFoundException_OnInvalidUserId()
    {
        // Arrange
        int userIdToUnfollow = 1;
        int currentUserId = 2;

        // Mock GetUserById to return null, simulating a user not found scenario
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(new List<User>());

        // Act and Assert
        var ex = await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _userService.Unfollow(userIdToUnfollow, currentUserId)
        );
        _outputHelper.WriteLine("" + ex);
    }

    [Fact]
    public async Task PutUser_ShouldThrowUserNotFoundException_OnInvalidUserId()
    {
        // Arrange
        int userIdToUpdate = 1;
        var userDto = new UserUpdateDTO()
        {
            // Set user properties
        };

        // Mock GetUserById to return null, simulating a user not found scenario
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(new List<User>());
        _userUpdateDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UserUpdateDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {}
            });

        // Act and Assert
        var ex = await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _userService.PutUser(userIdToUpdate, userDto)
        );
        _outputHelper.WriteLine("" + ex);
    }
    

    [Fact]
    public async Task PutUser_ShouldUpdateUser_WhenValidInputIsProvided()
    {
        // Arrange
        int userIdToUpdate = 1;
        var userDto = new UserUpdateDTO()
        {
            Location = "New York",
            Bio = "New bio",
            Avatar = new ImageDTO() { Url = "newavatar.jpg" },
        };
    
        var userToUpdate = new User
        {
            Id = userIdToUpdate,
            Email = "oldemail@example.com",
            Username = "oldusername",
            Location = "Old location",
            Bio = "Old bio",
            Avatar = null, // No avatar initially
        };

        var dbSetMock = new List<User>()
        {
            userToUpdate
        };
    
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(dbSetMock);
        _userUpdateDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UserUpdateDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {}
            });

        _applicationContextMock.Setup(c => c.Users.Update(It.IsAny<User>())).Callback<User>((User entity) =>
        {
            dbSetMock.Remove(userToUpdate);
            dbSetMock.Add(entity);
        });
        
        
    
       
    
        // Act
        await _userService.PutUser(userIdToUpdate, userDto);
    
        // Assert
        Assert.Equal(userDto.Location, dbSetMock.Single().Location);
        Assert.Equal(userDto.Bio, dbSetMock.Single().Bio);
        Assert.Equal(userDto.Avatar?.Url, dbSetMock.Single().Avatar?.Url);
    }
    
    [Fact]
    public async Task Follow_ShouldSuccessfullyFollowUser()
    {
        // Arrange
        int userIdToFollow = 1;
        int currentUserId = 2;

        var userToFollow = new User
        {
            Id = userIdToFollow,
            FollowersIds = new List<int>(), // Simulate that the user is not followed initially
        };

        var userThatFollows = new User
        {
            Id = currentUserId,
            FollowingIds = new List<int>(), // Simulate that the current user is not following anyone initially
        };

        List<User> users = new List<User>
        {
            userToFollow,
            userThatFollows,
        };

        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(users);

        // Act
        await _userService.Follow(userIdToFollow, currentUserId);

        // Assert
        Assert.Contains(currentUserId, userToFollow.FollowersIds);
        Assert.Contains(userIdToFollow, userThatFollows.FollowingIds);
    }
    
    [Fact]
    public async Task Follow_ShouldThrowInvalidOperationException_WhenAlreadyFollowing()
    {
        // Arrange
        int userIdToFollow = 1;
        int currentUserId = 2;

        var userToFollow = new User
        {
            Id = userIdToFollow,
            FollowersIds = new List<int> { currentUserId }, // Simulate that the user is already being followed
        };

        var userThatFollows = new User
        {
            Id = currentUserId,
            FollowingIds = new List<int>(), // Simulate that the current user is not following anyone initially
        };

        List<User> users = new List<User>
        {
            userToFollow,
            userThatFollows,
        };

        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(users);

        // Act and Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _userService.Follow(userIdToFollow, currentUserId)
        );
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task Unfollow_ShouldSuccessfullyUnfollowUser()
    {
        // Arrange
        int userIdToUnfollow = 1;
        int currentUserId = 2;

        var userToUnfollow = new User
        {
            Id = userIdToUnfollow,
            FollowersIds = new List<int> { currentUserId }, // Simulate that the user is being followed
        };

        var userThatUnfollows = new User
        {
            Id = currentUserId,
            FollowingIds = new List<int> { userIdToUnfollow }, // Simulate that the current user is following the target user
        };

        List<User> users = new List<User>
        {
            userToUnfollow,
            userThatUnfollows,
        };

        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(users);

        // Act
        await _userService.Unfollow(userIdToUnfollow, currentUserId);

        // Assert
        Assert.DoesNotContain(currentUserId, userToUnfollow.FollowersIds);
        Assert.DoesNotContain(userIdToUnfollow, userThatUnfollows.FollowingIds);
    }
    
    [Fact]
    public async Task Unfollow_ShouldThrowInvalidOperationException_WhenNotFollowing()
    {
        // Arrange
        int userIdToUnfollow = 1;
        int currentUserId = 2;

        var userToUnfollow = new User
        {
            Id = userIdToUnfollow,
            FollowersIds = new List<int>(), // Simulate that the user is not being followed
        };

        var userThatUnfollows = new User
        {
            Id = currentUserId,
            FollowingIds = new List<int>(), // Simulate that the current user is not following anyone initially
        };

        List<User> users = new List<User>
        {
            userToUnfollow,
            userThatUnfollows,
        };

        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(users);

        // Act and Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _userService.Unfollow(userIdToUnfollow, currentUserId)
        );
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public async Task DeleteUser_ShouldSuccessfullyDeleteUser()
    {
        // Arrange
        int userIdToDelete = 1;

        var userToDelete = new User
        {
            Id = userIdToDelete,
            // Set other user properties
        };

        List<User> users = new List<User>
        {
            userToDelete,
            // Add any other users needed for the test
        };
        
        
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(users);

        _applicationContextMock.Setup(c => c.Users.Remove(It.IsAny<User>()))
            .Callback<User>((User userEntity) =>
            {
                // Simulate the removal of the user from the database
                users.RemoveAll(u => u.Id == userEntity.Id);
            });

        // Act
        await _userService.DeleteUser(userIdToDelete);

        // Assert
        Assert.DoesNotContain(userToDelete, users);
    }

    [Fact]
    public async Task DeleteUser_ShouldThrowUserNotFoundException_WhenUserNotFound()
    {
        // Arrange
        int userIdToDelete = 1;

        // Simulate that the user does not exist in the database
        _applicationContextMock.Setup(c => c.Users).ReturnsDbSet(new List<User>());

        // Act and Assert
        var ex = await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _userService.DeleteUser(userIdToDelete)
        );
        _outputHelper.WriteLine("" + ex);
    }
}