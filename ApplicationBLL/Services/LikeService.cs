using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using AutoMapper;

namespace ApplicationBLL.Services;

public class LikeService : BaseService
{
    public LikeService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
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