using BlueSkySharp.Entities;

public interface IBlueSkyClient
{
    Task Authenticate(string username, string password, string domain = "https://bluesky.com");
    Task<Post> Post(string content);
}