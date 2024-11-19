
using BlueSkySharp.Entities;

using System.Net.Http.Json;
using System.Text;

namespace BlueSkySharp;

public class BlueSkyClient : IBlueSkyClient
{
    const string createSession = "xrpc/com.atproto.server.createSession";
    const string createRecord = "xrpc/com.atproto.repo.createRecord";
    private CreateSessionResponse? _createSessionResponse = null;
    private readonly HttpClient? _httpClient;
    private string? _pdsHost = string.Empty;
    /// <summary>
    /// Ceates a new instance of the BlueSkyClient class.
    /// </summary>
    public BlueSkyClient() : this(new HttpClient())
    {
    }

    /// <summary>
    /// Creates a new instance of the BlueSkyClient class.
    /// </summary>
    /// <param name="httpClient">HttpClient to use</param>
    public BlueSkyClient(HttpClient httpClient) => _httpClient = httpClient;

    /// <summary>
    /// This method authenticates the user with the BlueSky API.
    /// </summary>
    /// <param name="identifier">User's identifier</param>
    /// <param name="password">User's password</param>
    /// <param name="pdsHost">Domain to authenticate - optional. Default value(https://bsky.social/)</param>
    public async Task Authenticate(string identifier, string password, string pdsHost = "https://bsky.social/")
    {
        try
        {
            if (pdsHost.EndsWith('/'))
            {
                pdsHost = pdsHost[..^1];
            }

            _pdsHost = pdsHost;

            var createSessionEndpoint = $"{_pdsHost}/{createSession}";
            var createSessionRequest = new CreateSessionRequest()
            {
                Identifier = identifier,
                Password = password
            };
            var response = await _httpClient!.PostAsJsonAsync(createSessionEndpoint, createSessionRequest);
            response.EnsureSuccessStatusCode();
            _createSessionResponse = await response.Content.ReadFromJsonAsync<CreateSessionResponse>();
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }

    public async Task<Post> Post(string content)
    {
        _httpClient!.DefaultRequestHeaders.Add("Authorization", $"Bearer {_createSessionResponse!.AccessJwt}");
        var createRecordEndpoint = $"{_pdsHost}/{createRecord}";
        var createRecordRequest = new CreateRecordRequest()
        {
            Repo = _createSessionResponse.Handle,
            Collection = "app.bsky.feed.post",
            Record = new Record()
            {
                Text = content,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }
        };

        var response = await _httpClient.PostAsJsonAsync(createRecordEndpoint, createRecordRequest);
        response.EnsureSuccessStatusCode();
        var createRecordResponse = await response.Content.ReadFromJsonAsync<CreateRecordResponse>();

        return new Post()
        {
            Cid = createRecordResponse!.Cid,
            Uri= createRecordResponse!.Uri
        };
    }
}
