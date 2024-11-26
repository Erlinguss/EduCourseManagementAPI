using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using NuGet.Packaging.Signing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;


namespace EduCourseManagementTest
{
    public class StudentsTest : AuthenticatedTestBase
    {
        public StudentsTest(WebApplicationFactory<Program> factory) : base(factory) { }

        private StringContent GetJsonContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        [Fact]
        public async Task GetStudentById()
        {
            var client = await GetAuthorizedClientAsync();
            var response = await client.GetAsync("/api/Students/1");

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
        }

        [Fact]
        public async Task GetAllStudents()
        {
            var client = await GetAuthorizedClientAsync();
            var response = await client.GetAsync("/api/Students");

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
        }

        [Fact]
        public async Task PostStudent()
        {
            var client = await GetAuthorizedClientAsync();
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var newStudent = new
            {
                name = $"Joe{timestamp}",
                email = $"joe{timestamp}@example.com"
            };

            var response = await client.PostAsync("/api/Students?userId=1", GetJsonContent(newStudent));
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var student = JsonSerializer.Deserialize<StudentDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var generatedStudentId = student.StudentId;

            var deleteResponse = await client.DeleteAsync($"/api/Students/{generatedStudentId}");
        }

        [Fact]
        public async Task PutStudent()
        {
            var client = await GetAuthorizedClientAsync();
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var newStudent = new
            {
                name = $"Joe Rogan {Guid.NewGuid()}",
                email = $"roganJoe@dkit.com"
            };

            var postResponse = await client.PostAsync("/api/Students?userId=1", GetJsonContent(newStudent));
            Assert.Equal(System.Net.HttpStatusCode.Created, postResponse.StatusCode);

            var postContent = await postResponse.Content.ReadAsStringAsync();
            var createdStudent = JsonSerializer.Deserialize<StudentDTO>(postContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var generatedStudentId = createdStudent.StudentId;
            Console.WriteLine(generatedStudentId);

            var updatedStudent = new
            {
                studentId= generatedStudentId,
                name = "Joe Rogan",
                email = "roganJoe@dkit.com"
            };
            Console.WriteLine(updatedStudent);

            var putResponse = await client.PutAsync($"/api/Students/{generatedStudentId}", GetJsonContent(updatedStudent));
            Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);

            var deleteResponse = await client.DeleteAsync($"/api/Students/{generatedStudentId}");
        }


        }
    }
}