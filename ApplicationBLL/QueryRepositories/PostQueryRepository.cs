using System.Linq.Expressions;
using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories.Abstract;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.QueryRepositories;

public class PostQueryRepository : BaseQueryRepository
{
    public PostQueryRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }
    
    public async Task<IEnumerable<PostDTO>> GetAllPosts()
    {
        var posts = await _applicationContext.Posts.AsNoTracking()
            .Include(post => post.Author).ThenInclude(u => u.Avatar)
            .Include(p => p.Images).ToListAsync();
        
        return _mapper.Map<IEnumerable<PostDTO>>(posts);
    }

    public virtual async Task<PostDTO> GetPostById(int id, params Expression<Func<Post, object>>[] includeExpressions)
    {
        var query = _applicationContext.Posts.Include(p => p.Images)
            .Include(p => p.Author).ThenInclude(u => u.Avatar).AsNoTracking();

        var postModel = await query.FirstOrDefaultAsync(p => p.Id == id);

        if (postModel == null)
        {
            throw new PostNotFoundException();
        }

        return _mapper.Map<PostDTO>(postModel!);
    }
}