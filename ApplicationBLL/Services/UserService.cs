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
    public UserService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
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

        if (await _applicationContext.Users.AnyAsync(u => u.Email == userEntity.Email))
        {
            throw new UserAlreadyExistsException("User already exists");
        }
        
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