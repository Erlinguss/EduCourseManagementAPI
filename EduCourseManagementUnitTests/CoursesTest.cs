using EducationCourseManagement.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using Xunit;

public class CoursesControllerTests : AuthenticatedTestBase
{
    private int _generatedCourseId;

    public CoursesControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    private StringContent GetJsonContent(object obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    [Fact]
    public async Task GetCourseById()
    {

        var client = await GetAuthorizedClientAsync();
        var response = await client.GetAsync("/api/Courses/1");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
    }

    [Fact]
    public async Task GetAllCourses()
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.GetAsync("/api/Courses");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
    }

    [Fact]
    public async Task PostCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var newCourse = new
        {
            title = $"Data Science {timestamp}",
            description = "Prediction modules",
            credits = 4
        };

        var response = await client.PostAsync("/api/Courses", GetJsonContent(newCourse));
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var generatedCourseId = course.CourseId;
        var deleteResponse = await client.DeleteAsync($"/api/Courses/{generatedCourseId}");
    }

    [Fact]
    public async Task PutCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var newCourse = new
        {
            title = $"Data Science {Guid.NewGuid()}",
            description = "Prediction modules",
            credits = 4
        };

        var postResponse = await client.PostAsync("/api/Courses", GetJsonContent(newCourse));
        Assert.Equal(System.Net.HttpStatusCode.Created, postResponse.StatusCode);

        var postContent = await postResponse.Content.ReadAsStringAsync();
        var createdCourse = JsonSerializer.Deserialize<CourseDTO>(postContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var generatedCourseId = createdCourse.CourseId;


        var updatedCourse = new
        {
            title = $"Data Science ",
            description = "Advanced Prediction modules",
            credits = 8
        };

        var putResponse = await client.PutAsync($"/api/Courses/{generatedCourseId}", GetJsonContent(updatedCourse));
        Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);
        var deleteResponse = await client.DeleteAsync($"/api/Courses/{generatedCourseId}");
    }

    [Fact]
    public async Task DeleteCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var newCourse = new
        {
            title = $"Data Science{Guid.NewGuid()}",
            description = "Prediction modules",
            credits = 4
        };

        var postResponse = await client.PostAsync("/api/Courses", GetJsonContent(newCourse));
        Assert.Equal(System.Net.HttpStatusCode.Created, postResponse.StatusCode);

        var postContent = await postResponse.Content.ReadAsStringAsync();
        var createdCourse = JsonSerializer.Deserialize<CourseDTO>(postContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var generatedCourseId = createdCourse.CourseId;
        var deleteResponse = await client.DeleteAsync($"/api/Courses/{generatedCourseId}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}



