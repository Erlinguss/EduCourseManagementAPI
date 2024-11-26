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

    }
}