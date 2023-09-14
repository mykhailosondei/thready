using ApplicationBLL.Exceptions;
using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using ApplicationCommon.DTOs.User;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services;

public class UserService : BaseService
{
    private readonly EmailValidatorService _emailValidatorService;
    
    public UserService(ApplicationContext applicationContext, IMapper mapper, EmailValidatorService emailValidatorService) : base(applicationContext, mapper)
    {
        _emailValidatorService = emailValidatorService;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return _applicationContext.Users;
    }

    public async Task<User> GetUserById(int id)
    {
        var userModel = await _applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        return userModel;
    }
    
    public async Task Follow(int userId, int currentUserId)
    {
        var userToFollowModel = await GetUserById(userId);
        var userThatFollowsModel = await GetUserById(currentUserId);
        if (userToFollowModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        /*if (userThatFollowsModel.Following.Contains(userToFollowModel) || userToFollowModel.Followers.Contains(userThatFollowsModel))
        {
            throw new InvalidOperationException("You are already following this user");
        }
        
        userToFollowModel.Followers.Add(userThatFollowsModel);
        userThatFollowsModel.Following.Add(userToFollowModel);*/

        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task Unfollow(int userId, int currentUserId)
    {
        var userToUnfollowModel = await GetUserById(userId);
        var userThatUnfollowsModel = await GetUserById(currentUserId);

        if (userToUnfollowModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        /*if (!userThatUnfollowsModel.Following.Contains(userToUnfollowModel) ||
            !userToUnfollowModel.Followers.Contains(userThatUnfollowsModel))
        {
            throw new InvalidOperationException("You do not follow this user");
        }

        userToUnfollowModel.Followers.Remove(userThatUnfollowsModel);
        userThatUnfollowsModel.Following.Remove(userToUnfollowModel);*/

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<UserDTO> CreateUser(RegisterUserDTO registerUserDto)
    {
        var userEntity = _mapper.Map<User>(registerUserDto);

        if (!await _emailValidatorService.IsEmailAvailable(registerUserDto.Email))
        {
            throw new UserAlreadyExistsException("Email is already in use.");
        }

        /*userEntity.Posts = new List<Post>();
        userEntity.Followers = new List<User>();
        userEntity.Following = new List<User>();
        userEntity.Bio = "";
        userEntity.Location = "";
        userEntity.BookmarkedPosts = new List<Post>();
        userEntity.Reposts = new List<Post>();*/
        
        userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
        
        _applicationContext.Users.Add(userEntity);
        await _applicationContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(userEntity);
    }

    public async Task PutUser(int userId, UserDTO user)
    {
        var userToUpdate = await GetUserById(userId);
        
        if (userToUpdate == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        if (userToUpdate.Email != user.Email && await _emailValidatorService.IsEmailAvailable(user.Email))
        {
            userToUpdate.Email = user.Email;
        }
        
        

        if (user.Avatar != null || user.Avatar!.Url == "")
        {
            if (userToUpdate.Avatar == null)
            {
                userToUpdate.Avatar = new Image()
                {
                    Url = user.Avatar.Url,
                    Id = userToUpdate.Id
                };
            }
            else
            {
                userToUpdate.Avatar.Url = user.Avatar.Url;
            }
        }
        
        
        
    }

    public async Task DeleteUser(int id)
    {
        var userModel = await GetUserById(id);
        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }
        _applicationContext.Users.Remove(userModel);
        await _applicationContext.SaveChangesAsync();
    }
}