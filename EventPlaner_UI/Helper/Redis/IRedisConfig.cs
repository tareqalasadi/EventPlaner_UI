namespace EventPlaner_UI.Helper.Redis
{
    public interface IRedisConfig
    {
        Task<string> GetValueAsync(string key);
        Task SetValueAsync(string key, string value);
    }
}
