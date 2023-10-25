using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace group_project_thread.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class RecommendationController
{
    private readonly RecommendationService _recommendationService;
    
    
    public RecommendationController(RecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }
    
    [HttpGet("small_trends")]
    public async Task<IEnumerable<IndexedWord>> GetSmallTrends()
    {
        return await _recommendationService.GetCurrentPopularWords();
    }
    
    [HttpGet("trends")]
    public async Task<IEnumerable<IndexedWord>> GetTrends()
    {
        return await _recommendationService.GetMoreCurrentPopularWords();
    }
    
    [HttpGet("for_you/{userId}")]
    public async Task<IEnumerable<PostDTO>> GetPostsForYou(int userId)
    {
        return await _recommendationService.GetPostForYou(userId);
    }
    
    
    [HttpGet("who_to_follow")]
    public async Task<IEnumerable<PageUserDTO>> GetWhoToFollow()
    {
        return await _recommendationService.WhoToFollow();
    }
    
    [HttpGet("connect_people")]
    public async Task<IEnumerable<PageUserDTO>> GetConnectPeople()
    {
        return await _recommendationService.GetPeopleToConnectWith();
    }
    
    [HttpGet("creators_for_you")]
    public async Task<IEnumerable<PageUserDTO>> GetCreatorsForYou()
    {
        return await _recommendationService.GetCreatorsForYou();
    }
}