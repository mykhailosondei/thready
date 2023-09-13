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
        return await _applicationContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
    
    

    public async Task Follow(int userId, int currentUserId)
    {
        
    }
    
    public async Task Unfollow(int userId, int currentUserId)
    {
        
    }

    public async Task<UserDTO> CreateUser(RegisterUserDTO registerUserDto)
    {
        var userEntity = _mapper.Map<User>(registerUserDto);

        if (!await _emailValidatorService.IsEmailAvailable(registerUserDto.Email))
        {
            throw new UserAlreadyExistsException("Email is already in use.");
        }

        userEntity.Posts = new List<Post>();
        userEntity.Followers = new List<User>();
        userEntity.Following = new List<User>();
        userEntity.Bio = "";
        userEntity.Location = "";
        userEntity.BookmarkedPosts = new List<Post>();
        userEntity.Reposts = new List<Post>();
        
        userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
        
        _applicationContext.Users.Add(userEntity);
        await _applicationContext.SaveChangesAsync();

        return _mapper.Map<UserDTO>(userEntity);
    }

    public async Task PutUser(int id, User user)
    {
        
    }

    public async Task DeleteUser(int id)
    {
        _applicationContext.Users.Remove(await GetUserById(id));
        await _applicationContext.SaveChangesAsync();
    }
}