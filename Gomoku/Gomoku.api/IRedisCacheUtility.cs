namespace Gomoku.api;

public interface IRedisCacheUtility
{
    Task Set(string key, object value);
    Task<T?> Get<T>(string key);
    void Remove(string key);
    string GenerateHashKey(string text, string salt = "");
}

