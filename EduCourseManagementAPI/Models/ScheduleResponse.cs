namespace EducationCourseManagement.DTOs
{
    public class ScheduleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ScheduleDTO CreatedSchedule { get; set; }
        public ScheduleDTO ExistingSchedule { get; set; }
        public List<string> SuggestedTimeSlots { get; set; }
    }
}

