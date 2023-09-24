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
    private readonly UsernameValidatorService _usernameValidatorService;
    
    public UserService(ApplicationContext applicationContext, IMapper mapper, EmailValidatorService emailValidatorService, UsernameValidatorService usernameValidatorService) : base(applicationContext, mapper)
    {
        _emailValidatorService = emailValidatorService;
        _usernameValidatorService = usernameValidatorService;
    }

    public UserService() : base(null, null)
    {
        
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        return _applicationContext.Users.OrderBy(u => u.Id).Select(u => _mapper.Map<UserDTO>(u));
    }

    public virtual async Task<UserDTO> GetUserById(int id)
    {
        var userModel = await _applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        return _mapper.Map<UserDTO>(userModel);
    }
    
    public async Task Follow(int userId, int currentUserId)
    {
        var userToFollowModel = await GetUserById(userId);
        var userThatFollowsModel = await GetUserById(currentUserId);
        if (userToFollowModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        if (userThatFollowsModel.FollowingIds.Contains(userToFollowModel.Id) || userToFollowModel.FollowersIds.Contains(userThatFollowsModel.Id))
        {
            throw new InvalidOperationException("You are already following this user");
        }
        
        userToFollowModel.FollowersIds.Add(userThatFollowsModel.Id);
        userThatFollowsModel.FollowingIds.Add(userToFollowModel.Id);

        _applicationContext.Users.Update(_mapper.Map<User>(userToFollowModel));
        _applicationContext.Users.Update(_mapper.Map<User>(userThatFollowsModel));

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

        if (!userThatUnfollowsModel.FollowingIds.Contains(userToUnfollowModel.Id) ||
            !userToUnfollowModel.FollowersIds.Contains(userThatUnfollowsModel.Id))
        {
            throw new InvalidOperationException("You do not follow this user");
        }

        userToUnfollowModel.FollowersIds.Remove(userThatUnfollowsModel.Id);
        userThatUnfollowsModel.FollowingIds.Remove(userToUnfollowModel.Id);
        
        _applicationContext.Users.Update(_mapper.Map<User>(userToUnfollowModel));
        _applicationContext.Users.Update(_mapper.Map<User>(userThatUnfollowsModel));

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<UserDTO> CreateUser(RegisterUserDTO registerUserDto)
    {
        var userEntity = _mapper.Map<User>(registerUserDto);

        if (!await _emailValidatorService.IsEmailAvailable(registerUserDto.Email))
        {
            throw new UserAlreadyExistsException("Email is already in use.");
        }

        userEntity.Posts = new List<Post>();
        userEntity.FollowersIds = new List<int>();
        userEntity.FollowingIds = new List<int>();
        userEntity.Bio = "";
        userEntity.Location = "";
        userEntity.BookmarkedPostsIds = new List<int>();
        userEntity.RepostsIds = new List<int>();
        
        userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
        
        _applicationContext.Users.Add(userEntity);
        await _applicationContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(userEntity);
    }

    public virtual async Task PutUser(int userId, UserDTO user)
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
        
        if (userToUpdate.Username != user.Username && await _usernameValidatorService.IsUsernameAvailable(user.Username))
        {
            userToUpdate.Username = user.Username;
        }

        userToUpdate.Location = user.Location;
        userToUpdate.Bio = user.Bio;

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

        _applicationContext.Users.Update(_mapper.Map<User>(userToUpdate));
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var userModel = await GetUserById(id);
        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }
        _applicationContext.Users.Remove(_mapper.Map<User>(userModel));
        await _applicationContext.SaveChangesAsync();
    }
}