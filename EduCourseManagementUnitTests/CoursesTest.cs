using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class CoursesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CoursesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private StringContent GetJsonContent(object obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    [Fact]
    public async Task GetCourseById_ShouldReturnCourse()
    {
        var response = await _client.GetAsync("/api/Courses/1");
 
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
    }

    [Fact]
    public async Task PostCourse_ShouldAddCourse()
    {
        var newCourse = new
        {
            courseId = 11,
            title = "Data Science",
            description = "Prediction modules",
            credits = 4
        };

        var response = await _client.PostAsync("/api/Courses", GetJsonContent(newCourse));

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var locationHeader = response.Headers.Location?.ToString();
        Assert.NotNull(locationHeader);
        Assert.Contains("/api/Courses/11", locationHeader);
    }

    [Fact]
    public async Task GetAllCourses_ShouldReturnListOfCourses()
    {
        var response = await _client.GetAsync("/api/Courses");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
    }

 /*   [Fact]
    public async Task PutCourse_ShouldUpdateCourse()
    {
        var updatedCourse = new
        {
            courseId = 11,
            title = "Data Science",
            description = "Prediction modules",
            credits = 8
        };

        var response = await _client.PutAsync("/api/Courses/11", GetJsonContent(updatedCourse));

        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }*/


   [Fact]
    public async Task DeleteCourse_ShouldRemoveCourse()
    {
        var response = await _client.DeleteAsync("/api/Courses/11");

        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
