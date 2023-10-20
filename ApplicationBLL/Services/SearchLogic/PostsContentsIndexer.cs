using System.Text.RegularExpressions;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services.SearchLogic;

public class PostsContentsIndexer
{
    private readonly IndexerContext _indexerContext;
    
    public PostsContentsIndexer(IndexerContext indexerContext)
    {
        _indexerContext = indexerContext;
    }

    public async Task AddIndexedWordsToTableByPostId(int id, string textContent)
    {
        var words = await _indexerContext.IndexedWords.ToListAsync();
        var wordsFrequencyInPost = GetFrequencyOfWords(textContent);
        foreach (var wordFrequency in wordsFrequencyInPost)
        {
            var existingWord = words.FirstOrDefault(word => word.Word == wordFrequency.Key);
            if (existingWord != null)
            {
                _indexerContext.Attach(existingWord);
                await _indexerContext.Entry(existingWord).Collection(w => w.WordCountInPostId).LoadAsync();
                existingWord.WordCountInPostId.Add(new WordCountInPostId()
                {
                    PostId = id,
                    WordCount = wordFrequency.Value
                });
                existingWord.PostsCount++;
                _indexerContext.Entry(existingWord).Collection(e => e.WordCountInPostId).IsModified = true;
            }
            else
            {
                var wordFrequencyByPosts = new List<WordCountInPostId>();
                wordFrequencyByPosts.Add(new WordCountInPostId()
                {
                    PostId = id,
                    WordCount = wordFrequency.Value
                });
                var newWord = new IndexedWord()
                {
                    PostsCount = 1,
                    Word = wordFrequency.Key,
                    WordCountInPostId = wordFrequencyByPosts
                };

                _indexerContext.IndexedWords.Add(newWord);
            }
        }
        Console.WriteLine(DateTime.Now);
        await _indexerContext.SaveChangesAsync();
    }
    
    
    private Dictionary<string, int> GetFrequencyOfWords(string words)
    {
        var result = new Dictionary<string, int>();
        string[] separateWords = TextContentSplit(words.ToLower());
        foreach (var word in separateWords)
        {
            if (word == "")
            {
                continue;
            }
            string wordTrimmed = word.Trim();
            if (!result.ContainsKey(wordTrimmed))
                result.Add(wordTrimmed, 1);
            else
                result[wordTrimmed]++;
        }
        return result;
    }
    private string[] TextContentSplit(string textContent)
    {
        string pattern = @"[^a-zA-Z'-]+";

        return Regex.Split(textContent, pattern);
    }
}