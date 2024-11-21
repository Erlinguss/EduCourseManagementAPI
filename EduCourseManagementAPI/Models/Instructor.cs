using EducationCourseManagement.Models;
using EduCourseManagementAPI.Models;

public class Instructor : BaseEntity
{
    public int InstructorId { get; set; }

    // Navigation property for schedules
    public ICollection<Schedule> Schedules { get; set; }
    public User User { get; set; }
}
