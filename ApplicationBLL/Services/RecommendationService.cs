using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services.Abstract;
using ApplicationBLL.Services.SearchLogic;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services;

public class RecommendationService : BaseService
{
    private readonly IndexerContext _indexerContext;
    private readonly IndexedContentReader _indexedContentReader;
    private readonly UserQueryRepository _userQueryRepository;
    
    public RecommendationService(IndexerContext indexerContext, ApplicationContext applicationContext,
        IndexedContentReader indexedContentReader, IMapper mapper, UserQueryRepository userQueryRepository) : base(applicationContext, mapper)
    {
        _indexerContext = indexerContext;
        _indexedContentReader = indexedContentReader;
        _userQueryRepository = userQueryRepository;
    }

    public async Task<List<IndexedWord>> GetCurrentPopularWords()
    {
        var words = await _indexerContext.IndexedWords
            .OrderByDescending(w => w.PostsCount)
            .Take(3)
            .ToListAsync();
        return words;
    }
    public async Task<List<IndexedWord>> GetMoreCurrentPopularWords()
    {
        var words = await _indexerContext.IndexedWords
            .OrderByDescending(w => w.PostsCount)
            .Take(30)
            .ToListAsync();
        return words;
    }
    
    public async Task<List<PostDTO>> GetPostForYou(int userId)
    {
        var user = await _userQueryRepository.GetUserById(userId);
        List<PostDTO> result = new List<PostDTO>();
        var postsBasedOnLocation = await _indexedContentReader.GetPosts(user.Location, 0, 10);
        var postsBasedOnBio = await _indexedContentReader.GetPosts(user.Bio, 0, 10);
        result.AddRange(postsBasedOnLocation);
        result.AddRange(postsBasedOnBio);
        var popularPosts = await _applicationContext.Posts.Include(p => p.Author).OrderByDescending(p => p.ViewedBy)
            .Take(30-result.Count).ToListAsync();
        var postsDTOs = popularPosts.Select(p => _mapper.Map<PostDTO>(p));
        result.AddRange(postsDTOs);
        return result;
    }
    public async Task<List<PageUserDTO>> WhoToFollow()
    {
        var currentUser = await _userQueryRepository.GetUserById(_userQueryRepository.GetCurrentUserId());
        var people = await _applicationContext.Users
            .Include(u=>u.Avatar).
            OrderByDescending(u => u.FollowersIds.Count)
            .Where(u => u.Id != currentUser.Id && !currentUser.FollowingIds.Contains(u.Id))
            .Take(3)
            .Select(u => _mapper.Map<PageUserDTO>(u))
            .ToListAsync();
        return people;
    }
    
    public async Task<List<PageUserDTO>> GetPeopleToConnectWith()
    {
        var currentUser = await _userQueryRepository.GetUserById(_userQueryRepository.GetCurrentUserId());
        var userCount = _applicationContext.Users.Count();
        userCount = userCount - 50 < 0 ? 0 : new Random().Next(50, userCount);
        var people = await _applicationContext.Users
            .Include(u=>u.Avatar).
            OrderByDescending(u => u.FollowersIds.Count)
            .Where(u => u.Id != currentUser.Id && !currentUser.FollowingIds.Contains(u.Id))
            .Skip(userCount)
            .Take(50)
            .Select(u => _mapper.Map<PageUserDTO>(u))
            .ToListAsync();
        return people;
    }
    
    public async Task<List<PageUserDTO>> GetCreatorsForYou()
    {
        var currentUser = await _userQueryRepository.GetUserById(_userQueryRepository.GetCurrentUserId());
        var userCount = _applicationContext.Users.Count();
        userCount = userCount - 5 < 0 ? 0 : new Random().Next(5, userCount/4);
        var people = await _applicationContext.Users
            .Include(u=>u.Avatar).
            OrderByDescending(u => u.FollowersIds.Count)
            .Where(u => u.Id != currentUser.Id && !currentUser.FollowingIds.Contains(u.Id))
            .Skip(userCount)
            .Take(5)
            .Select(u => _mapper.Map<PageUserDTO>(u))
            .ToListAsync();
        return people;
    }

}