namespace EducationCourseManagement.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }

        // Navigation properties
        public ICollection<Schedule> Schedules { get; set; }
    }
}