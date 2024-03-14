using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using Norka.API.Data;
using Norka.API.Entities;

namespace Norka.API.Services;

public class ApiTokenService(NorkaDbContext dbContext, ILogger<ApiTokenService> logger)
{
    /// <summary>
    /// Get all api tokens for given user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ApiToken>> GetApiTokensAsync(string userId)
    {
        var apiTokens = await dbContext.ApiTokens
            .Where(x => x.UserId == userId)
            .ToListAsync();
        return apiTokens;
    }

    /// <summary>
    /// Get api token by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiToken?> GetApiTokenAsync(string id)
    {
        var apiToken = await dbContext.ApiTokens
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return apiToken;
    }

    /// <summary>
    /// Get api token for given user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ApiToken?> GetApiTokenAsync(string id, string userId)
    {
        var apiToken = await dbContext.ApiTokens
            .Where(x => x.Id == id && x.UserId == userId)
            .FirstOrDefaultAsync();
        return apiToken;
    }

    /// <summary>
    /// Create new api token and return it.
    /// Current user will be the author.
    /// Token will be expired after <paramref name="expiryInDays"/>
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="title"></param>
    /// <param name="expiryInDays"></param>
    /// <returns></returns>
    public async Task<ApiToken> CreateApiTokenAsync(string userId, string title, int expiryInDays)
    {
        var apiToken = new ApiToken
        {
            Id = await Nanoid.GenerateAsync(),
            Title = title,
            UserId = userId,
            Token = await Nanoid.GenerateAsync(size: 32),
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(expiryInDays)
        };
        dbContext.ApiTokens.Add(apiToken);
        await dbContext.SaveChangesAsync();
        return apiToken;
    }

    /// <summary>
    /// Delete api token by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(string id)
    {
        var apiToken = await dbContext.ApiTokens
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (apiToken != null)
        {
            dbContext.ApiTokens.Remove(apiToken);
            await dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Delete api token by token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task DeleteAsync(ApiToken token)
    {
        dbContext.ApiTokens.Remove(token);
        await dbContext.SaveChangesAsync();
    }
}