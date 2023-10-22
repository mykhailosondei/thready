using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories.Abstract;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.QueryRepositories;

public class UserQueryRepository : BaseQueryRepository
{

    private readonly IUserIdGetter _userIdGetter;
    
    public UserQueryRepository(ApplicationContext applicationContext, IMapper mapper, IUserIdGetter userIdGetter) : base(applicationContext, mapper)
    {
        _userIdGetter = userIdGetter;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        return _applicationContext.Users.OrderBy(u => u.Id).Select(u => _mapper.Map<UserDTO>(u));
    }
    
    public int GetCurrentUserId()
    {
        return _userIdGetter.CurrentId;
    }

    public virtual async Task<UserDTO> GetUserById(int id)
    {
        var userModel = await _applicationContext.Users.Include(u=> u.Avatar).FirstOrDefaultAsync(u => u.Id == id);

        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified id does not exist");
        }

        return _mapper.Map<UserDTO>(userModel);
    }
    
    public virtual async Task<UserDTO> GetUserByUsername(string username)
    {
        var userModel = await _applicationContext.Users.Include(u => u.Avatar).FirstOrDefaultAsync(u => u.Username == username);

        if (userModel == null)
        {
            throw new UserNotFoundException("User with specified username does not exist");
        }

        return _mapper.Map<UserDTO>(userModel);
    }
}