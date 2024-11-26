using EducationCourseManagement.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }

    // Navigation properties
    public ICollection<Schedule> Schedules { get; set; }
    public ICollection<StudentCourse> StudentCourses { get; set; }
}
