using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class PostService : BaseService
{
    public PostService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }
    
    public async Task<IEnumerable<PostDTO>> GetAllPosts()
    {
        return default;
    }

    public async Task<PostDTO> GetPostById(int id)
    {
        return default;
    }
    
    public async Task<IEnumerable<PostDTO>> GetPostsByUserId(int id)
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

    public async Task CreatePost(PostCreateDTO post)
    {
        
    }

    public async Task PutPost(int id, PostDTO post)
    {
        
    }

    public async Task DeletePost(int id)
    {
        
    }
}