using EduCourseManagementAPI.Models;

public class Register : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } 
}
