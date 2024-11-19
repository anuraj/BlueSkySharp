using BlueSkySharp.Entities;

using Moq;
using Moq.Protected;

using System.Net;
using System.Text;
using System.Text.Json;

namespace BlueSkySharp.Tests;

public class BlueSkySharpUnitTests
{
    [Fact]
    public async Task TestAuthenticate()
    {
        var jwtExample = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.dyt0CoTl4WoVjAHI9Q_CwSKhl6d";
        var createSessionResponse = new CreateSessionResponse
        {
            AccessJwt = jwtExample,
            Active = true,
            Email = "dev@example.com",
            EmailAuthFactor = false,
            EmailConfirmed = true,
            Handle = "dev",
            RefreshJwt = jwtExample
        };

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(createSessionResponse), Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new BlueSkyClient(httpClient);
        await client.Authenticate("Handle", "Password");

        mockHandler.Protected().Verify("SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task TestPost()
    {
        var jwtExample = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.dyt0CoTl4WoVjAHI9Q_CwSKhl6d";
        var createSessionResponse = new CreateSessionResponse
        {
            AccessJwt = jwtExample,
            Active = true,
            Email = "dev@example.com",
            EmailAuthFactor = false,
            EmailConfirmed = true,
            Handle = "dev",
            RefreshJwt = jwtExample
        };

        var createRecordResponse = new CreateRecordResponse
        {
            Uri = "https://bsky.social/1234567890",
            Cid = "1234567890",
            Commit = new Commit()
            {
                Cid = "1234567890",
                Rev = "1",
            },
            ValidationStatus = "valid"
        };

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(m => m.RequestUri!.AbsolutePath.EndsWith("com.atproto.server.createSession")),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(createSessionResponse), Encoding.UTF8, "application/json")
            });

        mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(m => m.RequestUri!.AbsolutePath.EndsWith("com.atproto.repo.createRecord")),
            ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(createRecordResponse), Encoding.UTF8, "application/json")
                });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new BlueSkyClient(httpClient);
        await client.Authenticate("Handle", "Password");
        await client.Post("Hello, World!");

        mockHandler.Protected().Verify("SendAsync",
            Times.Exactly(2),
            ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>());
    }
}