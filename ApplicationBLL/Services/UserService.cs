using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using ApplicationCommon.DTOs.User;
using AutoMapper;

namespace ApplicationBLL.Services;

public class UserService : BaseService
{
    public UserService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
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

    public async Task<UserDTO> CreateUser(RegisterUserDTO registerUserDto)
    {
        var userEntity = _mapper.Map<User>(registerUserDto);
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
        
    }
}