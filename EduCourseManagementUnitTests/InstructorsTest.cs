using EducationCourseManagement.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace EduCourseManagementTest
{
    public class InstructorsTest : AuthenticatedTestBase
    {
        public InstructorsTest(WebApplicationFactory<Program> factory) : base(factory) { }

        private StringContent GetJsonContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        [Fact]
        public async Task GetInstructorById()
        {
            var client = await GetAuthorizedClientAsync();
            var response = await client.GetAsync("/api/Instructors/1");

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
        }

        [Fact]
        public async Task GetAllInstructors()
        {
            var client = await GetAuthorizedClientAsync();
            var response = await client.GetAsync("/api/Instructors");

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content should not be null or empty");
        }

        [Fact]
        public async Task PostInstructor()
        {
            var client = await GetAuthorizedClientAsync();
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var newInstructor = new
            {
                name = $"John Doe {timestamp}",
                email = $"john.doe{timestamp}@example.com"
            };

            var response = await client.PostAsync("/api/Instructors?userId=1", GetJsonContent(newInstructor));
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var instructor = JsonSerializer.Deserialize<InstructorDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var generatedInstructorId = instructor.InstructorId;

            var deleteResponse = await client.DeleteAsync($"/api/Instructors/{generatedInstructorId}");
        }

       
    }
}
