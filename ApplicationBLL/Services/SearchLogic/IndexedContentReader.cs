using System.Text.RegularExpressions;
using ApplicationBLL.QueryRepositories;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services.SearchLogic;

public class IndexedContentReader
{
    private readonly IndexerContext _indexerContext;
    private readonly PostQueryRepository _postQueryRepository;
    private readonly UserQueryRepository _userQueryRepository;
    public IndexedContentReader(IndexerContext indexerContext, PostQueryRepository postQueryRepository, UserQueryRepository userQueryRepository)
    {
        _indexerContext = indexerContext;
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
    }

    public async Task<IEnumerable<PostDTO>> GetPosts(string query)
    {
        var postsToLoad = await FindPostsIds(query);
        if (postsToLoad.Count == 0)
        {
            throw new Exception("No posts found");
        }

        List<PostDTO> matchingPosts = new List<PostDTO>();
        foreach (var wordCountInPostId in postsToLoad)
        {
            try
            {
                 var potentialPost = await _postQueryRepository.GetPostById(wordCountInPostId.PostId);
                 matchingPosts.Add(potentialPost);
            }
            catch (Exception e)
            {
            }
        }

        return matchingPosts;
    }

    private async Task<List<WordCountInPostId>> FindPostsIds(string query)
    {
        string pattern = @"[^a-zA-Z'-]+";
        var words = await _indexerContext.IndexedWords.ToListAsync();
        string[] queryWords = Regex.Split(query, pattern);
        List<WordCountInPostId> matchingWordCountInPostIds = new List<WordCountInPostId>();
        foreach (var word in queryWords)
        {
            var possibleWord = words.FirstOrDefault(w => w.Word == word);
            
            if (possibleWord != null)
            {
                _indexerContext.Attach(possibleWord);
                await _indexerContext.Entry(possibleWord).Collection(w => w.WordCountInPostId).LoadAsync();
                var sorted = possibleWord.WordCountInPostId.OrderByDescending(w => w.WordCount);
                return sorted.ToList();
            }
            
            var similarWords = words.Where(w => IsSimilar(w.Word, query)).ToList();
            foreach (var similarWord in similarWords)
            {
                _indexerContext.Attach(similarWord);
                await _indexerContext.Entry(similarWord).Collection(w => w.WordCountInPostId).LoadAsync();
                var sorted = similarWord.WordCountInPostId.OrderByDescending(w => w.WordCount);
                matchingWordCountInPostIds.AddRange(sorted);
            }

            if (matchingWordCountInPostIds.Count > 0)
            {
                return matchingWordCountInPostIds;
            }
        }

        return new List<WordCountInPostId>();
    }

    
    
    public async Task<IEnumerable<UserDTO>> GetUsers(string query)
    {
        var usersToLoad = await FindUsersIds(query);
        if (usersToLoad.Count == 0)
        {
            throw new Exception("No users found");
        }

        List<UserDTO> matchingUsers = new List<UserDTO>();
        foreach (var wordCountInPostId in usersToLoad)
        {
            try
            {
                var potentialUser = await _userQueryRepository.GetUserById(wordCountInPostId.UserId);
                matchingUsers.Add(potentialUser);
            }
            catch (Exception e)
            {
            }
        }

        return matchingUsers;
    }
    
    async Task<List<IndexedUsername>> FindUsersIds(string query)
    {
        var usernames = await _indexerContext.IndexedUsernames.ToListAsync();
        string pattern = @"[^a-zA-Z\d-]+";
        string[] queryWords = Regex.Split(query, pattern);
        List<IndexedUsername> result = new List<IndexedUsername>();
        foreach (var word in queryWords)
        {
            var possibleUsername = usernames.FirstOrDefault(u => u.Username  == word);
            
            if (possibleUsername != null)
            {
                result.Add(possibleUsername);
            }

            var similarUsernames = usernames.Where(u => IsSimilar(u.Username, word)).ToList();
            result.AddRange(similarUsernames);
            var partialUsernames = usernames.Where(u => u.Username.Contains(word));
            result.AddRange(partialUsernames);
            if (result.Count != 0)
                return result;
        }

        return result;
    }

    bool IsSimilar(string word, string query)
    {
        int maxDistance = 1;
        if (Math.Abs(word.Length - query.Length) > maxDistance)
            return false;
        int[,] similarityDistance = new int[word.Length + 1, query.Length + 1];
        for (int i = 0; i <= word.Length; i++)
        {
            similarityDistance[i,0] = i;
        }

        for (int i = 0; i <= query.Length; i++)
        {
            similarityDistance[0, i] = i;
        }

        int minNumberInRow = maxDistance;
        for (int row = 1; row <= word.Length; row++)
        {
            minNumberInRow = similarityDistance[row - 1, query.Length];
            for (int col = 1; col <= query.Length; col++)
            {
                if (word[row - 1] == query[col - 1])
                {
                    similarityDistance[row, col] = similarityDistance[row - 1, col - 1];
                    minNumberInRow = minNumberInRow > similarityDistance[row - 1, col - 1] ? similarityDistance[row -1, col -1] : minNumberInRow ;
                }
                else
                {
                    var min = Math.Min(Math.Min(similarityDistance[row - 1, col - 1], similarityDistance[row - 1, col]),
                        similarityDistance[row, col - 1]);
                    similarityDistance[row, col] = min + 1;
                    minNumberInRow = min + 1 < minNumberInRow ? min + 1 : minNumberInRow;
                }
            }
            if (minNumberInRow > maxDistance)
                return false;
        }
        return similarityDistance[word.Length, query.Length] <= maxDistance;
    }
}