using EduCourseManagementAPI.Models;

namespace EducationCourseManagement.Models
{
    public class Student : BaseEntity
    {
        public int StudentId { get; set; }
 
        // Navigation property for the many-to-many relationship with Courses
        public ICollection<StudentCourse> StudentCourses { get; set; }
        public User User { get; set; }
    }
}