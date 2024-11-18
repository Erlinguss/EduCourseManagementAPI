namespace EducationCourseManagement.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
    }
}
