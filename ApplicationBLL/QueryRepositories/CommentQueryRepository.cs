using System.Linq.Expressions;
using ApplicationBLL.Exceptions;
using ApplicationBLL.Extentions;
using ApplicationBLL.QueryRepositories.Abstract;
using ApplicationCommon.DTOs.Comment;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace ApplicationBLL.QueryRepositories;

public class CommentQueryRepository : BaseQueryRepository
{
    
    public CommentQueryRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }
    
    public async Task<CommentDTO> GetCommentByIdPlain(int id, params Expression<Func<Comment, object>>[] includeExpressions)
    {
        var query = _applicationContext.Comments.AsNoTracking().Include(c => c.Author).ThenInclude(u => u.Avatar)
            .Include(c => c.Images);

        //query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

        var comment = await query.FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            throw new CommentNotFoundException("Comment with specified id not found");
        }

        return _mapper.Map<CommentDTO>(comment);
    }

    public virtual async Task<CommentDTO> GetCommentWithAllCommentTreeById(int id)
    {

        var clock = new SystemClock();
        Console.WriteLine(clock.UtcNow.ToString());
        int depth = 0;
        int currentCheckingId = id;
        var commentForDepthChecking = await _applicationContext.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == currentCheckingId);
        
        if (commentForDepthChecking == null)
        {
            throw new CommentNotFoundException("Comment with specified id not found");
        }

        while (commentForDepthChecking.CommentId != null)
        {
            depth++;
            currentCheckingId = commentForDepthChecking.CommentId.Value;
            commentForDepthChecking = await _applicationContext.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == currentCheckingId);
        }

        Console.WriteLine(clock.UtcNow.ToString());

        Console.WriteLine(depth);
        
        var comment = await _applicationContext.Comments
            .Include(c => c.Author).ThenInclude(u => u.Avatar)
            .Include(c => c.Images)
            .CustomInclude(depth)
            .FirstOrDefaultAsync(c => c.Id == id);


        if (depth != 0)
        {
            for (Comment iterationComment = comment.ParentComment; iterationComment != null; iterationComment = iterationComment.ParentComment)
            {
                _applicationContext.Attach(iterationComment);
                await _applicationContext.Entry(iterationComment).Collection(c => c.Images).LoadAsync();
            }
        }
        
        return _mapper.Map<CommentDTO>(comment);
    }
    
}