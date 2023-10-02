using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories.Abstract;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.QueryRepositories;

public class UserQueryRepository : BaseQueryRepository
{
    
    public UserQueryRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
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
}