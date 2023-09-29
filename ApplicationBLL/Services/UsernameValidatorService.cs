using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services;

public class UsernameValidatorService : BaseService
{
    public UsernameValidatorService() : base(null, null)
    {
        
    }   
    
    public UsernameValidatorService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }

    public virtual async Task<bool> IsUsernameAvailable(string username)
    {
        if (username.Length > 15 || username == "")
        {
            return false;
        }
        return !await _applicationContext.Users.AnyAsync(u => u.Username == username);
    }
}