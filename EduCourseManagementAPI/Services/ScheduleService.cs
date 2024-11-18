using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationCourseManagement.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly SchoolContext _context;

        public ScheduleService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleDTO>> GetAllSchedulesAsync()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                .ToListAsync();

            return schedules.Select(s => new ScheduleDTO
            {
                ScheduleId = s.ScheduleId,
                CourseId = s.CourseId,
                InstructorId = s.InstructorId,
                Date = s.Date,
                TimeSlot = s.TimeSlot
            });
        }

        public async Task<ScheduleDTO> GetScheduleByIdAsync(int id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                .FirstOrDefaultAsync(s => s.ScheduleId == id);

            if (schedule == null) return null;

            return new ScheduleDTO
            {
                ScheduleId = schedule.ScheduleId,
                CourseId = schedule.CourseId,
                InstructorId = schedule.InstructorId,
                Date = schedule.Date,
                TimeSlot = schedule.TimeSlot
            };
        }

        public async Task<ScheduleDTO> CreateScheduleAsync(ScheduleDTO scheduleDTO)
        {
            var schedule = new Schedule
            {
                CourseId = scheduleDTO.CourseId,
                InstructorId = scheduleDTO.InstructorId,
                Date = scheduleDTO.Date,
                TimeSlot = scheduleDTO.TimeSlot
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            scheduleDTO.ScheduleId = schedule.ScheduleId;
            return scheduleDTO;
        }

        /* public async Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO)
         {
             var schedule = await _context.Schedules.FindAsync(id);

             if (schedule == null) return false;

             schedule.CourseId = scheduleDTO.CourseId;
             schedule.InstructorId = scheduleDTO.InstructorId;
             schedule.Date = scheduleDTO.Date;
             schedule.TimeSlot = scheduleDTO.TimeSlot;

             _context.Schedules.Update(schedule);
             await _context.SaveChangesAsync();

             return true;
         }*/

        public async Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
                return false;

            // Validate foreign keys
            var courseExists = await _context.Courses.AnyAsync(c => c.CourseId == scheduleDTO.CourseId);
            var instructorExists = await _context.Instructors.AnyAsync(i => i.InstructorId == scheduleDTO.InstructorId);

            if (!courseExists || !instructorExists)
                throw new InvalidOperationException("Invalid CourseId or InstructorId.");

            schedule.CourseId = scheduleDTO.CourseId;
            schedule.InstructorId = scheduleDTO.InstructorId;
            schedule.Date = scheduleDTO.Date;
            schedule.TimeSlot = scheduleDTO.TimeSlot;

            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null) return false;

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
