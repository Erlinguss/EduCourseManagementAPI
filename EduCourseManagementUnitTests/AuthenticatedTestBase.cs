using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public abstract class AuthenticatedTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;

    public AuthenticatedTestBase(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private StringContent GetJsonContent(object obj) =>
        new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    protected async Task<string> LoginAndGetTokenAsync()
    {
        var loginCredentials = new
        {
            username = "PeterAdmin",
            password = "admindkit"
        };

        var response = await _client.PostAsync("/api/Auth/Login", GetJsonContent(loginCredentials));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tokenObject = JsonSerializer.Deserialize<TokenResponse>(content);

        return tokenObject?.Token ?? throw new Exception("Failed to retrieve JWT token.");
    }

    protected async Task<HttpClient> GetAuthorizedClientAsync()
    {
        var token = await LoginAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return _client;
    }

    private class TokenResponse
    {
        public string Token { get; set; }
    }
}
