using Microsoft.Extensions.Caching.Distributed;
using System.Security.Cryptography;
using System.Text.Json;

namespace Gomoku.api;

public class RedisCacheUtility : IRedisCacheUtility
{

    private readonly IDistributedCache _distributedCache;
    public RedisCacheUtility(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task Set(string key, object? value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        // serialize the board and save in Redis cache
        string cachedString = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, cachedString);
    }

    public async Task<T?> Get<T>(string key)
    {
        string cachedString = await _distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cachedString))
        {
            T? value = JsonSerializer.Deserialize<T>(cachedString);
            return value;

        }

        return default(T);
    }


    public void Remove(string key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        _distributedCache.Remove(key);
    }

    /// <summary>
    /// generates unique hash key
    /// </summary>
    /// <param name="text"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public string GenerateHashKey(string text, string salt = "")
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        // Uses SHA256 to create the hash
        using (var sha = SHA256.Create())
        {
            // Convert the string to a byte array first, to be processed
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            // Convert back to a string, removing the '-' that BitConverter adds
            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }
}

