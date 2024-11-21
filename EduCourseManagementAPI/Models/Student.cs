namespace EducationCourseManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int UserId { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }

        // Navigation property for the many-to-many relationship with Courses
        public ICollection<StudentCourse> StudentCourses { get; set; }
        public User User { get; set; }
    }
}