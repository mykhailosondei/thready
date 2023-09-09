using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;

namespace ApplicationBLL.Services;

public class LikeService : BaseService
{
    public LikeService(ApplicationContext applicationContext) : base(applicationContext)
    {
        
    }
    
    public async Task LikePost(int postId, int authorId)
    {
        
    }

    public async Task DislikePost(int postId, int authorId)
    {
        
    }

    public async Task LikeComment(int commentId, int authorId)
    {
        
    }
    
    public async Task DislikeComment(int commentId, int authorId)
    {
        
    }
}