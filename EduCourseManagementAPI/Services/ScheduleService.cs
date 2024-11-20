using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
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
            try
            {
                var schedules = await _context.Schedules
                    .Include(s => s.Course)
                    .Include(s => s.Instructor)
                    .Include(s => s.Room)
                    .ToListAsync();

                return schedules.Select(s => new ScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    CourseId = s.CourseId,
                    InstructorId = s.InstructorId,
                    RoomId = s.RoomId,
                    Date = s.Date,
                    TimeSlot = s.TimeSlot
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all schedules: {ex.Message}", ex);
            }
        }

        public async Task<ScheduleDTO> GetScheduleByIdAsync(int id)
        {
            try
            {
                var schedule = await _context.Schedules
                    .Include(s => s.Course)
                    .Include(s => s.Instructor)
                    .Include(s => s.Room)
                    .FirstOrDefaultAsync(s => s.ScheduleId == id);

                if (schedule == null)
                    throw new KeyNotFoundException($"Schedule with ID {id} not found.");

                return new ScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    CourseId = schedule.CourseId,
                    InstructorId = schedule.InstructorId,
                    RoomId = schedule.RoomId,
                    Date = schedule.Date,
                    TimeSlot = schedule.TimeSlot
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching schedule by ID: {ex.Message}", ex);
            }
        }

        public async Task<ScheduleDTO> CreateScheduleAsync(ScheduleDTO scheduleDTO)
        {
            try
            {
                // Validate room availability
                var isRoomAvailable = await _context.Schedules.AnyAsync(s =>
                    s.RoomId == scheduleDTO.RoomId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot);
                if (isRoomAvailable)
                    throw new InvalidOperationException("The room is already booked for the specified time slot.");

                // Validate instructor availability
                var isInstructorAvailable = await _context.Schedules.AnyAsync(s =>
                    s.InstructorId == scheduleDTO.InstructorId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot);
                if (isInstructorAvailable)
                    throw new InvalidOperationException("The instructor is already booked for the specified time slot.");

                // Validate course availability
                var isCourseAvailable = await _context.Schedules.AnyAsync(s =>
                    s.CourseId == scheduleDTO.CourseId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot);
                if (isCourseAvailable)
                    throw new InvalidOperationException("The course is already scheduled for the specified time slot.");

                var schedule = new Schedule
                {
                    CourseId = scheduleDTO.CourseId,
                    InstructorId = scheduleDTO.InstructorId,
                    RoomId = scheduleDTO.RoomId,
                    Date = scheduleDTO.Date,
                    TimeSlot = scheduleDTO.TimeSlot
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();

                scheduleDTO.ScheduleId = schedule.ScheduleId;
                return scheduleDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating schedule: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO)
        {
            try
            {
                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null)
                    throw new KeyNotFoundException($"Schedule with ID {id} not found.");

                // Revalidate room availability
                var isRoomAvailable = await _context.Schedules.AnyAsync(s =>
                    s.RoomId == scheduleDTO.RoomId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot &&
                    s.ScheduleId != id);
                if (isRoomAvailable)
                    throw new InvalidOperationException("The room is already booked for the specified time slot.");

                // Revalidate instructor availability
                var isInstructorAvailable = await _context.Schedules.AnyAsync(s =>
                    s.InstructorId == scheduleDTO.InstructorId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot &&
                    s.ScheduleId != id);
                if (isInstructorAvailable)
                    throw new InvalidOperationException("The instructor is already booked for the specified time slot.");

                // Revalidate course availability
                var isCourseAvailable = await _context.Schedules.AnyAsync(s =>
                    s.CourseId == scheduleDTO.CourseId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot &&
                    s.ScheduleId != id);
                if (isCourseAvailable)
                    throw new InvalidOperationException("The course is already scheduled for the specified time slot.");

                schedule.CourseId = scheduleDTO.CourseId;
                schedule.InstructorId = scheduleDTO.InstructorId;
                schedule.RoomId = scheduleDTO.RoomId;
                schedule.Date = scheduleDTO.Date;
                schedule.TimeSlot = scheduleDTO.TimeSlot;

                _context.Schedules.Update(schedule);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating schedule: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            try
            {
                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null)
                    throw new KeyNotFoundException($"Schedule with ID {id} not found.");

                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting schedule: {ex.Message}", ex);
            }
        }
    }
}