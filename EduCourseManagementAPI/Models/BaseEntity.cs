namespace EduCourseManagementAPI.Models
{
    public abstract class BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
