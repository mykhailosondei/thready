using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services.SearchLogic;

public class UsernamesIndexer
{
    private readonly IndexerContext _indexerContext;

    public UsernamesIndexer(IndexerContext indexerContext)
    {
        _indexerContext = indexerContext;
    }

    public async Task AddIndexedUsername(int id, string username)
    {
        var usernames = await _indexerContext.IndexedUsernames.ToListAsync();
        IndexedUsername newUsername = new IndexedUsername()
        {
            UserId = id,
            Username = username
        };
        _indexerContext.IndexedUsernames.Add(newUsername);
        await _indexerContext.SaveChangesAsync();
    }
}