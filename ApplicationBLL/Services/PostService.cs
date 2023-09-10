using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class PostService : BaseService
{
    public PostService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }
    
    public async Task<IEnumerable<Post>> GetAllPosts()
    {
        return default;
    }

    public async Task<Post> GetPostById(int id)
    {
        return default;
    }
    
    public async Task<IEnumerable<Post>> GetPostsByUserId(int id)
    {
        return default;
    }

    public async Task BookmarkPost(int postId, int userId)
    {
        
    }
    
    public async Task RemoveFromBookmarksPost(int postId, int userId)
    {
        
    }

    public async Task Repost(int postId, int userId)
    {
        
    }
    
    public async Task UndoRepost(int postId, int userId)
    {
        
    }

    public async Task CreatePost(Post post)
    {
        
    }

    public async Task PutPost(int id, Post post)
    {
        
    }

    public async Task DeletePost(int id)
    {
        
    }
}