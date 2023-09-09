using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;

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

    public async Task PostUser(User user)
    {
        
    }

    public async Task PutUser(int id, User user)
    {
        
    }

    public async Task DeleteUser(int id)
    {
        
    }
}