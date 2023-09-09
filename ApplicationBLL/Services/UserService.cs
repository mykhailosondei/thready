using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using ApplicationCommon.DTOs.User;

namespace ApplicationBLL.Services;

public class UserService : BaseService
{
    public UserService(ApplicationContext applicationContext) : base(applicationContext)
    {
        
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return default;
    }

    public async Task<User> GetUserById(int id)
    {
        return default;
    }

    public async Task Follow(int userId, int currentUserId)
    {
        
    }
    
    public async Task Unfollow(int userId, int currentUserId)
    {
        
    }

    public async Task<User> CreateUser(RegisterUserDTO registerUserDto)
    {
        var userEntity = new User(){Username = registerUserDto.UserName, Email = registerUserDto.Email, DateOfBirth = registerUserDto.DateOfBirth};
        userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
        
        _applicationContext.Users.Add(userEntity);
        await _applicationContext.SaveChangesAsync();

        return userEntity;
    }

    public async Task PutUser(int id, User user)
    {
        
    }

    public async Task DeleteUser(int id)
    {
        
    }
}