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
    public async Task GetCourseById_ShouldReturnCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.GetAsync($"/api/Courses/1");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
    }

    [Fact]
    public async Task GetAllCourses_ShouldReturnListOfCourses()
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.GetAsync("/api/Courses");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
    }


    private async Task<int> CreateCourseAndGetIdAsync()
    {
        var client = await GetAuthorizedClientAsync();
        var newCourse = new
        {
            title = "Data Science",
            description = "Prediction modules",
            credits = 4
        };

        var response = await client.PostAsync("/api/Courses", GetJsonContent(newCourse));
        response.EnsureSuccessStatusCode(); 

        var content = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return course.CourseId;
    }

    [Fact]
    public async Task PostCourse_ShouldAddCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var newCourse = new
        {
            title = "Data Science",
            description = "Prediction modules",
            credits = 4
        };

        var response = await client.PostAsync("/api/Courses", GetJsonContent(newCourse));
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _generatedCourseId = course.CourseId; 
        Assert.True(_generatedCourseId > 0, "CourseId should be greater than 0");
    }

    [Fact]
    public async Task PutCourse_ShouldUpdateCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var updatedCourse = new
        {
            title = "Updated Data Science",
            description = "Advanced Prediction modules",
            credits = 8
        };

        var response = await client.PutAsync($"/api/Courses/{_generatedCourseId}", GetJsonContent(updatedCourse));
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCourse_ShouldRemoveCourse()
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.DeleteAsync($"/api/Courses/{_generatedCourseId}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
