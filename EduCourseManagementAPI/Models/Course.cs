using EducationCourseManagement.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }

    // Navigation property for related schedules
    public ICollection<Schedule> Schedules { get; set; }

    // Navigation property for the many-to-many relationship with Students
    public ICollection<StudentCourse> StudentCourses { get; set; }

}
