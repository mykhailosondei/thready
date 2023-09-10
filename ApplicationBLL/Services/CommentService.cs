using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class CommentService : BaseService
{
    public CommentService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }

    public async Task<Comment> GetCommentById(int id)
    {
        return default;
    }
    
    public async Task<IEnumerable<Comment>> GetCommentsOfPostId(int postId)
    {
        return default;
    }

    public async Task PostComment(Comment Comment)
    {
        
    }

    public async Task PutComment(int id, Comment Comment)
    {
        
    }

    public async Task DeleteComment(int id)
    {
        
    }
}