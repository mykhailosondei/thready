using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ApplicationBLL.Exceptions;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services.Abstract;
using ApplicationBLL.Services.SearchLogic;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;


namespace ApplicationBLL.Services;

public class PostService : BaseService
{
    private readonly PostQueryRepository _postQueryRepository;
    private readonly UserQueryRepository _userQueryRepository;
    private readonly IValidator<PostDTO> _postValidator;
    private readonly IValidator<PostUpdateDTO> _postUpdateValidator;
    private readonly PostsContentsIndexer _postsContentsIndexer;
    
    public PostService(ApplicationContext applicationContext, IMapper mapper, IValidator<PostDTO> postValidator, 
        PostQueryRepository postQueryRepository, UserQueryRepository userQueryRepository, IValidator<PostUpdateDTO> postUpdateValidator, 
        PostsContentsIndexer postsContentsIndexer) : base(applicationContext, mapper)
    {
        _postValidator = postValidator;
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _postUpdateValidator = postUpdateValidator;
        _postsContentsIndexer = postsContentsIndexer;
    }

    
    protected PostService(PostsContentsIndexer postsContentsIndexer) : base(null, null)
    {
        _postsContentsIndexer = postsContentsIndexer;
    }

    
    public async Task<IEnumerable<PostDTO>> GetPostsByUserId(int id)
    {
        var result = await _applicationContext.Posts
            .Include(p => p.Author).ThenInclude(u => u.Avatar)
            .Include(p => p.Images)
            .Where(p => p.Author.Id == id)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<PostDTO>>(result);
    }

    public async Task BookmarkPost(int postId, int userId)
    {
        var userModel = await _userQueryRepository.GetUserById(userId);
        var post = await _postQueryRepository.GetPostById(postId);
        if (await DoesPostExist(postId))
        {
            throw new PostNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedPostsIds.Contains(postId);

        if (isBookmarked)
            throw new InvalidOperationException("Already bookmarked");
        
        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);

        postEntity.Bookmarks++;
        userEntity.BookmarkedPostsIds.Add(postId);
        await BookmarkEntitiesSaveChanges(userEntity, postEntity);
    }
    
    public async Task RemoveFromBookmarksPost(int postId, int userId)
    {
        var userModel = await _userQueryRepository.GetUserById(userId);
        var post = await _postQueryRepository.GetPostById(postId);
        if (await DoesPostExist(postId))
        {
            throw new PostNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedPostsIds.Contains(postId);

        if (!isBookmarked)
        {
            throw new InvalidOperationException("Not bookmarked");
        }

        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);

        postEntity.Bookmarks--;
        userEntity.BookmarkedPostsIds.Remove(postId);
        await BookmarkEntitiesSaveChanges(userEntity, postEntity);
    }

    public async Task Repost(int postId, int userId)
    {
        var userModel = await _userQueryRepository.GetUserById(userId);
        var post = await _postQueryRepository.GetPostById(postId);

        bool isReposted = userModel.RepostsIds.Contains(postId);

        if (isReposted)
        {
            throw new InvalidOperationException("Already reposted");
        }

        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);
        
        userEntity.RepostsIds.Add(postId);
        postEntity.RepostersIds.Add(userId);

        await RepostEntitiesSaveChanges(userEntity, postEntity);
    }

    private async Task BookmarkEntitiesSaveChanges(User userEntity, Post postEntity)
    {
        _applicationContext.Attach(userEntity); 
        _applicationContext.Entry(userEntity).Property(u => u.BookmarkedPostsIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
        _applicationContext.ChangeTracker.Clear();
        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(postEntity).Property(p => p.Bookmarks).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }

    private async Task RepostEntitiesSaveChanges(User userEntity, Post postEntity)
    {
        _applicationContext.Attach(userEntity);
        _applicationContext.Entry(userEntity).Property(u => u.RepostsIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
        _applicationContext.ChangeTracker.Clear();
        
        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(postEntity).Property(p => p.RepostersIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task UndoRepost(int postId, int userId)
    {
        var userModel = await _userQueryRepository.GetUserById(userId);
        var post = await _postQueryRepository.GetPostById(postId);

        bool isReposted = userModel.RepostsIds.Contains(postId);

        if (!isReposted)
        {
            throw new InvalidOperationException("Not reposted");
        }
        
        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);
        
        userEntity.RepostsIds.Remove(postId);
        postEntity.RepostersIds.Remove(userId);

        await RepostEntitiesSaveChanges(userEntity, postEntity);
    }

    public async Task<PostDTO> CreatePost(PostCreateDTO post)
    {
        var postEntity = _mapper.Map<Post>(post);
        var postDTO = _mapper.Map<PostDTO>(postEntity); 
        ValidationResult validationResult = await _postValidator.ValidateAsync(postDTO);

        if (post.Images.Count == 0 && await _applicationContext.Posts.AnyAsync(p => p.TextContent == post.TextContent))
        {
            throw new InvalidOperationException("Post with this content already exists");
        }

        if (!validationResult.IsValid)
        {
            throw new EmptyPostException();
        }

        InitPost(ref postEntity);
        
        _applicationContext.Posts.Add(postEntity);
        _applicationContext.Attach(postEntity.Author);
        if (postEntity.Author.Avatar != null)
        {
            _applicationContext.Attach(postEntity.Author.Avatar);
        }
        postEntity.Author.PostsCount++;
        
        await _applicationContext.SaveChangesAsync();
        await _postsContentsIndexer.AddIndexedWordsToTableByPostId(postEntity.Id, postEntity.TextContent);
        return _mapper.Map<PostDTO>(postEntity);
    }

    private void InitPost(ref Post postEntity)
    {
        postEntity.CreatedAt = DateTime.UtcNow;
        postEntity.LikesIds = new List<int>();
        postEntity.ViewedBy = new List<int>();
        postEntity.CommentsIds = new List<int>();
        postEntity.Bookmarks = 0;
        postEntity.RepostersIds = new List<int>();
    }

    private Func<Image, string> imageSelector = image => image.Url;

    public virtual async Task PutPost(int id, PostUpdateDTO post)
    {
        var postToUpdate = await _postQueryRepository.GetPostById(id, p => p.Author);
        
        await _postsContentsIndexer.ChangeIndexedWordsPostsCountByPostId(postToUpdate.TextContent);
        await _postsContentsIndexer.RemoveWordsCountsFromTableByPostId(postToUpdate.Id);
        
        var currentUserId = _userQueryRepository.GetCurrentUserId();
        
        if (postToUpdate.Author.Id != currentUserId)
        {
            throw new InvalidOperationException("You are not the author of this post");
        }
        
        ValidationResult validationResult = await _postUpdateValidator.ValidateAsync(post);

        if (!validationResult.IsValid)
        {
            throw new EmptyPostException();
        }

        var postEntity = _mapper.Map<Post>(postToUpdate);
        var updatedImages = post.Images.Select(i => _mapper.Map<Image>(i)).ToList();

        _applicationContext.Attach(postEntity);

        await _applicationContext.Entry(postEntity).Collection(p => p.Images).LoadAsync();

        var imagesToAdd = updatedImages.ExceptBy(postEntity.Images.Select(imageSelector), imageSelector).ToList();
        var imagesToDelete = postEntity.Images.ExceptBy(updatedImages.Select(imageSelector), imageSelector).ToList();
        
        postEntity.Images.AddRange(imagesToAdd.DistinctBy(imageSelector));

        foreach (var image in imagesToDelete)
        {
            postEntity.Images.RemoveAll(i => i.Url == image.Url);
            _applicationContext.Entry(image).State = EntityState.Deleted;
        }
        
        postEntity.TextContent = post.TextContent;

        _applicationContext.Entry(postEntity).Collection(p => p.Images).IsModified = true;
        _applicationContext.Entry(postEntity).Property(p => p.TextContent).IsModified = true;
        await _applicationContext.SaveChangesAsync();
        await _postsContentsIndexer.AddIndexedWordsToTableByPostId(postEntity.Id, postEntity.TextContent);
    }

    public async Task DeletePost(int postId)
    {
        var post = await _postQueryRepository.GetPostById(postId, (p) => p.Author);
        await _postsContentsIndexer.ChangeIndexedWordsPostsCountByPostId(post.TextContent);
        await _postsContentsIndexer.RemoveWordsCountsFromTableByPostId(post.Id);
        var postEntity = _mapper.Map<Post>(post);
        _applicationContext.Posts.Remove(postEntity);
        
        await _applicationContext.SaveChangesAsync();
        _applicationContext.Attach(postEntity.Author);
        postEntity.Author.PostsCount--;
        _applicationContext.Entry(postEntity.Author).Property(u => u.PostsCount).IsModified = true;
        
        var reposterUsers = post.RepostersIds.Select(async i => _mapper.Map<User>(await _userQueryRepository.GetUserById(i)));
        
        foreach (var reposterUser in reposterUsers)
        {
            var awaitedUser = await reposterUser;
            awaitedUser.RepostsIds.Remove(postId);
            _applicationContext.Attach(awaitedUser);
            _applicationContext.Entry(awaitedUser).Property(u => u.RepostsIds).IsModified = true;
        }
        await _applicationContext.SaveChangesAsync();
    }

    private async Task<bool> DoesPostExist(int postId)
    {
        return await _applicationContext.Posts.FirstOrDefaultAsync(p => p.Id == postId) == null;
    }

    
    // implement post viewing
    
    public async Task ViewPost(int id, int authorId)
    {
        var post = await _postQueryRepository.GetPostById(id);

        bool isViewed = post.ViewedBy.Contains(authorId);

        if (isViewed) return;
        
        var postEntity = _mapper.Map<Post>(post);
        postEntity.ViewedBy.Add(authorId);
        await ViewEntitiesSaveChanges(postEntity);
    }
    
    private async Task ViewEntitiesSaveChanges(Post postEntity)
    {
        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(postEntity).Property(p => p.ViewedBy).IsModified = true;
        await _applicationContext.SaveChangesAsync();    
    }
}