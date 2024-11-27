using System.Text;
using System.Text.Json;
using EducationCourseManagement.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public abstract class AuthenticatedTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;
    private const string ApiKey = "S3cR3tK3yForMyAPI@2024*EducationCourse#Management";

    public AuthenticatedTestBase(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    protected StringContent GetJsonContent(object obj) =>
        new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    protected async Task<string> LoginAndGetTokenAsync()
    {
        var loginRequest = new
        {
            username = "PeterAdmin",
            password = "admindkit",
            role = "Admin"
        };

        // Add API Key header to the request
        if (!_client.DefaultRequestHeaders.Contains("ApiKey"))
        {
            _client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
        }

        var response = await _client.PostAsync("/api/Auth/Login", GetJsonContent(loginRequest));

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Login failed. Status Code: {response.StatusCode}, Message: {errorMessage}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Token ?? throw new InvalidOperationException("Token not found in the login response.");
    }

    protected async Task<HttpClient> GetAuthorizedClientAsync()
    {
        var token = await LoginAndGetTokenAsync();

        // Add Authorization header
        if (!_client.DefaultRequestHeaders.Contains("Authorization"))
        {
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // Add API Key header
        if (!_client.DefaultRequestHeaders.Contains("ApiKey"))
        {
            _client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
        }

        return _client;
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
