using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.Comment;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class CommentService : BaseService
{
    public CommentService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }

    public async Task<CommentDTO> GetCommentById(int id)
    {
        return default;
    }
    
    public async Task<IEnumerable<CommentDTO>> GetCommentsOfPostId(int postId)
    {
        return default;
    }

    public async Task PostComment(CommentCreateDTO Comment)
    {
        
    }

    public async Task PutComment(int id, CommentDTO Comment)
    {
        
    }

    public async Task DeleteComment(int id)
    {
        
    }
}