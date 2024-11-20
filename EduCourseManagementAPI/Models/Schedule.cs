namespace EducationCourseManagement.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }

        // Navigation properties
        public Course Course { get; set; }
        public Instructor Instructor { get; set; }
        public Room Room { get; set; }
    }
}
