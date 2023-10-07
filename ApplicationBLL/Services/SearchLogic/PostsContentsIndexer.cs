using System.Text.RegularExpressions;
using ApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services.SearchLogic;

public class PostsContentsIndexer
{
    private readonly IndexerContext _indexerContext; 
    
    public PostsContentsIndexer(IndexerContext indexerContext)
    {
        _indexerContext = indexerContext;
    }

    private string[] TextContentSplit(string textContent)
    {
        string pattern = @"[^a-zA-Z']+";

        return Regex.Split(textContent, pattern);
    }

    public Dictionary<string, int> WordFrequency(string words)
    {
        var result = new Dictionary<string, int>();
        string[] separateWords = TextContentSplit(words);
        foreach (var word in separateWords)
        {
            if (!result.ContainsKey(word))
                result.Add(words, 1);
            else
                result[word]++;
        }
        return result;
    }

    private List<(int, int)> BinarySearch(List<(int, int)> wordCountByPostIds, (int, int) wordCountByPostId )
    {
        
        int leftBound = 0;
        int rightBound = wordCountByPostIds.Count - 1;
        int midlleIndex = 0;
        while (leftBound <= rightBound)
        {
            midlleIndex = leftBound + (rightBound - leftBound) / 2;
            if (wordCountByPostId.Item1 > wordCountByPostIds[midlleIndex].Item1)
            {
                rightBound = midlleIndex - 1;
            }
            else
            {
                leftBound = midlleIndex + 1;
            }
        }
        
        wordCountByPostIds.Insert(leftBound, wordCountByPostId);
        return wordCountByPostIds;
    }
}