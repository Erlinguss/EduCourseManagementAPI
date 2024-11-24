/*using EducationCourseManagement.Data;
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
                var schedule = await ValidatedScheduleAsync(
                    scheduleDTO.CourseId,
                    scheduleDTO.InstructorId,
                    scheduleDTO.RoomId,
                    scheduleDTO.Date,
                    scheduleDTO.TimeSlot
                );

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
                var existingSchedule = await _context.Schedules.FindAsync(id);
                if (existingSchedule == null)
                    throw new KeyNotFoundException($"Schedule with ID {id} not found.");

                // Validate the new schedule details (without removing the existing schedule)
                var isRoomAvailable = await _context.Schedules.AnyAsync(s =>
                    s.RoomId == scheduleDTO.RoomId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot &&
                    s.ScheduleId != id);
                if (isRoomAvailable)
                    throw new InvalidOperationException("The room is already booked for the specified time slot.");

                var isInstructorAvailable = await _context.Schedules.AnyAsync(s =>
                    s.InstructorId == scheduleDTO.InstructorId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot &&
                    s.ScheduleId != id);
                if (isInstructorAvailable)
                    throw new InvalidOperationException("The instructor is already booked for the specified time slot.");

                var isCourseAvailable = await _context.Schedules.AnyAsync(s =>
                    s.CourseId == scheduleDTO.CourseId &&
                    s.Date.Date == scheduleDTO.Date.Date &&
                    s.TimeSlot == scheduleDTO.TimeSlot &&
                    s.ScheduleId != id);
                if (isCourseAvailable)
                    throw new InvalidOperationException("The course is already scheduled for the specified time slot.");

                // Update the existing schedule with new values
                existingSchedule.CourseId = scheduleDTO.CourseId;
                existingSchedule.InstructorId = scheduleDTO.InstructorId;
                existingSchedule.RoomId = scheduleDTO.RoomId;
                existingSchedule.Date = scheduleDTO.Date;
                existingSchedule.TimeSlot = scheduleDTO.TimeSlot;

                _context.Schedules.Update(existingSchedule);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Validation failed: {ex.Message}", ex);
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




        private readonly List<string> _timeSlots = new()
        {
            "9:00 AM - 10:30 AM", "10:30 AM - 12:00 PM", "1:00 PM - 2:30 PM","2:30 PM - 4:00 PM"
        };

        public async Task<ScheduleDTO> ValidatedScheduleAsync(int courseId, int instructorId, int roomId, DateTime date, string timeSlot)
        {
            try
            {
                // Validate room availability
                var isRoomAvailable = await _context.Schedules.AnyAsync(s =>
                    s.RoomId == roomId &&
                    s.Date.Date == date.Date &&
                    s.TimeSlot == timeSlot);
                if (isRoomAvailable)
                    throw new InvalidOperationException($"Room {roomId} is already booked for {date:yyyy-MM-dd} at {timeSlot}.");

                // Validate instructor availability
                var isInstructorAvailable = await _context.Schedules.AnyAsync(s =>
                    s.InstructorId == instructorId &&
                    s.Date.Date == date.Date &&
                    s.TimeSlot == timeSlot);
                if (isInstructorAvailable)
                    throw new InvalidOperationException($"Instructor {instructorId} is already booked for {date:yyyy-MM-dd} at {timeSlot}.");

                // Validate course availability
                var isCourseAvailable = await _context.Schedules.AnyAsync(s =>
                    s.CourseId == courseId &&
                    s.Date.Date == date.Date &&
                    s.TimeSlot == timeSlot);
                if (isCourseAvailable)
                    throw new InvalidOperationException($"Course {courseId} is already scheduled for {date:yyyy-MM-dd} at {timeSlot}.");

                // Create a new schedule
                var schedule = new Schedule
                {
                    CourseId = courseId,
                    InstructorId = instructorId,
                    RoomId = roomId,
                    Date = date,
                    TimeSlot = timeSlot
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();

                // Map to ScheduleDTO
                return new ScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    CourseId = schedule.CourseId,
                    InstructorId = schedule.InstructorId,
                    RoomId = schedule.RoomId,
                    Date = schedule.Date,
                    TimeSlot = schedule.TimeSlot,
                    IsAutoGenerated = true,
                    GeneratedBy = "ValidatedScheduleAsync"
                };
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Validation failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the schedule: {ex.Message}", ex);
            }
        }


        public async Task<bool> GenerateSchedulesForDayAsync(DateTime date)
        {
            try
            {
                // Fetch all required entities
                var courses = await _context.Courses.ToListAsync();
                var instructors = await _context.Instructors.ToListAsync();
                var rooms = await _context.Rooms.ToListAsync();

                foreach (var course in courses)
                {
                    foreach (var timeSlot in _timeSlots)
                    {
                        // Intelligent assignment: Find available instructor and room
                        var availableInstructor = instructors.FirstOrDefault(i =>
                            !_context.Schedules.Any(s =>
                                s.InstructorId == i.InstructorId &&
                                s.Date.Date == date.Date &&
                                s.TimeSlot == timeSlot));

                        var availableRoom = rooms.FirstOrDefault(r =>
                            !_context.Schedules.Any(s =>
                                s.RoomId == r.RoomId &&
                                s.Date.Date == date.Date &&
                                s.TimeSlot == timeSlot));

                        // Skip if no instructor or room available for the time slot
                        if (availableInstructor == null || availableRoom == null)
                            continue;

                        // Create schedule for the course
                        var schedule = new Schedule
                        {
                            CourseId = course.CourseId,
                            InstructorId = availableInstructor.InstructorId,
                            RoomId = availableRoom.RoomId,
                            Date = date,
                            TimeSlot = timeSlot
                        };

                        _context.Schedules.Add(schedule);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating schedules for {date:yyyy-MM-dd}: {ex.Message}", ex);
            }
        }


    }
}
*/


/*using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
using EduCourseManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<ScheduleDTO> GetScheduleByIdAsync(int id)
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

        public async Task<ScheduleDTO> CreateScheduleAsync(ScheduleDTO scheduleDTO)
        {
            var schedule = await ValidatedScheduleAsync(
                scheduleDTO.CourseId,
                scheduleDTO.InstructorId,
                scheduleDTO.RoomId,
                scheduleDTO.Date,
                scheduleDTO.TimeSlot
            );

            scheduleDTO.ScheduleId = schedule.ScheduleId;
            return scheduleDTO;
        }

        public async Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO)
        {
            var existingSchedule = await _context.Schedules.FindAsync(id);
            if (existingSchedule == null)
                throw new KeyNotFoundException($"Schedule with ID {id} not found.");

            // Validate new details while ignoring the current schedule's ID
            var hasConflict = await _context.Schedules.AnyAsync(s =>
                s.Date.Date == scheduleDTO.Date.Date &&
                s.TimeSlot == scheduleDTO.TimeSlot &&
                (s.RoomId == scheduleDTO.RoomId ||
                 s.InstructorId == scheduleDTO.InstructorId ||
                 s.CourseId == scheduleDTO.CourseId) &&
                s.ScheduleId != id);

            if (hasConflict)
                throw new InvalidOperationException("The updated schedule conflicts with an existing schedule.");

            // Update fields
            existingSchedule.CourseId = scheduleDTO.CourseId;
            existingSchedule.InstructorId = scheduleDTO.InstructorId;
            existingSchedule.RoomId = scheduleDTO.RoomId;
            existingSchedule.Date = scheduleDTO.Date;
            existingSchedule.TimeSlot = scheduleDTO.TimeSlot;

            _context.Schedules.Update(existingSchedule);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {id} not found.");

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<List<string>> GetAvailableTimeSlotsAsync(DateTime date)
        {
            var allTimeSlots = new List<string>
            {
                "9:00 AM - 10:30 AM", "10:30 AM - 12:00 PM",
                "1:00 PM - 2:30 PM", "2:30 PM - 4:00 PM"
            };

            var bookedTimeSlots = await _context.Schedules
                .Where(s => s.Date.Date == date.Date)
                .Select(s => s.TimeSlot)
                .ToListAsync();

            return allTimeSlots.Except(bookedTimeSlots).ToList();
        }

        public async Task<bool> GenerateSchedulesForDayAsync(DateTime date)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var courses = await _context.Courses.ToListAsync();
                var instructors = await _context.Instructors.ToListAsync();
                var rooms = await _context.Rooms.ToListAsync();

                foreach (var course in courses)
                {
                    // Create a fresh copy of available time slots for each course
                    var availableTimeSlots = await GetAvailableTimeSlotsAsync(date);

                    foreach (var timeSlot in availableTimeSlots.ToList()) 
                    {
                        var availableInstructor = instructors.FirstOrDefault(i =>
                            !_context.Schedules.Any(s =>
                                s.InstructorId == i.InstructorId &&
                                s.Date.Date == date.Date &&
                                s.TimeSlot == timeSlot));

                        var availableRoom = rooms.FirstOrDefault(r =>
                            !_context.Schedules.Any(s =>
                                s.RoomId == r.RoomId &&
                                s.Date.Date == date.Date &&
                                s.TimeSlot == timeSlot));

                        if (availableInstructor == null || availableRoom == null)
                            continue;

                        // Create a schedule entry for this course
                        var schedule = new Schedule
                        {
                            CourseId = course.CourseId,
                            InstructorId = availableInstructor.InstructorId,
                            RoomId = availableRoom.RoomId,
                            Date = date,
                            TimeSlot = timeSlot
                        };

                        _context.Schedules.Add(schedule);
                        availableTimeSlots.Remove(timeSlot);
                        break;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Failed to generate schedules for {date:yyyy-MM-dd}: {ex.Message}");
            }
        }


        public async Task<ScheduleDTO> ValidatedScheduleAsync(int courseId, int instructorId, int roomId, DateTime date, string timeSlot)
        {
            var conflicts = await _context.Schedules.AnyAsync(s =>
                s.Date.Date == date.Date &&
                s.TimeSlot == timeSlot &&
                (s.RoomId == roomId || s.InstructorId == instructorId || s.CourseId == courseId));

            if (conflicts)
                throw new InvalidOperationException("Conflict detected for the given schedule details.");

            var schedule = new Schedule
            {
                CourseId = courseId,
                InstructorId = instructorId,
                RoomId = roomId,
                Date = date,
                TimeSlot = timeSlot
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

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
    }
}*/

using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EducationCourseManagement.Models;
using EduCourseManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationCourseManagement.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly SchoolContext _context;

        public ScheduleService(SchoolContext context)
        {
            _context = context;
        }

        // Get all schedules
        public async Task<IEnumerable<ScheduleDTO>> GetAllSchedulesAsync()
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

        // Get schedule by ID
        public async Task<ScheduleDTO> GetScheduleByIdAsync(int id)
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

        // Create a new schedule
        public async Task<ScheduleResponse> CreateScheduleAsync(ScheduleDTO scheduleDTO)
        {
            var (isConflict, message, existingSchedule, suggestedTimeSlots) = await ValidatedScheduleAsync(
                scheduleDTO.CourseId,
                scheduleDTO.InstructorId,
                scheduleDTO.RoomId,
                scheduleDTO.Date,
                scheduleDTO.TimeSlot
            );

            if (isConflict)
            {
                return new ScheduleResponse
                {
                    Success = false,
                    Message = message,
                    ExistingSchedule = existingSchedule != null ? new ScheduleDTO
                    {
                        ScheduleId = existingSchedule.ScheduleId,
                        CourseId = existingSchedule.CourseId,
                        InstructorId = existingSchedule.InstructorId,
                        RoomId = existingSchedule.RoomId,
                        Date = existingSchedule.Date,
                        TimeSlot = existingSchedule.TimeSlot
                    } : null,
                    SuggestedTimeSlots = suggestedTimeSlots
                };
            }

            var schedule = new Schedule
            {
                CourseId = scheduleDTO.CourseId,
                InstructorId = scheduleDTO.InstructorId,
                RoomId = scheduleDTO.RoomId,
                Date = scheduleDTO.Date,
                TimeSlot = scheduleDTO.TimeSlot,
                IsAutoGenerated = scheduleDTO.IsAutoGenerated,
                GeneratedBy = scheduleDTO.GeneratedBy
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return new ScheduleResponse
            {
                Success = true,
                Message = "Schedule created successfully.",
                CreatedSchedule = new ScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    CourseId = schedule.CourseId,
                    InstructorId = schedule.InstructorId,
                    RoomId = schedule.RoomId,
                    Date = schedule.Date,
                    TimeSlot = schedule.TimeSlot,
                    IsAutoGenerated = schedule.IsAutoGenerated,
                    GeneratedBy = schedule.GeneratedBy
                }
            };
        }

        // Update an existing schedule
        public async Task<bool> UpdateScheduleAsync(int id, ScheduleDTO scheduleDTO)
        {
            var existingSchedule = await _context.Schedules.FindAsync(id);
            if (existingSchedule == null)
                throw new KeyNotFoundException($"Schedule with ID {id} not found.");

            var hasConflict = await _context.Schedules.AnyAsync(s =>
                s.Date.Date == scheduleDTO.Date.Date &&
                s.TimeSlot == scheduleDTO.TimeSlot &&
                (s.RoomId == scheduleDTO.RoomId ||
                 s.InstructorId == scheduleDTO.InstructorId ||
                 s.CourseId == scheduleDTO.CourseId) &&
                s.ScheduleId != id);

            if (hasConflict)
                throw new InvalidOperationException("The updated schedule conflicts with an existing schedule.");

            existingSchedule.CourseId = scheduleDTO.CourseId;
            existingSchedule.InstructorId = scheduleDTO.InstructorId;
            existingSchedule.RoomId = scheduleDTO.RoomId;
            existingSchedule.Date = scheduleDTO.Date;
            existingSchedule.TimeSlot = scheduleDTO.TimeSlot;

            _context.Schedules.Update(existingSchedule);
            await _context.SaveChangesAsync();

            return true;
        }

        // Delete a schedule by ID
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {id} not found.");

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return true;
        }

        // Validate conflicts
        public async Task<(bool IsConflict, string Message, Schedule ExistingSchedule, List<string> SuggestedTimeSlots)> ValidatedScheduleAsync(
            int courseId, int instructorId, int roomId, DateTime date, string timeSlot)
        {
            var conflictingSchedule = await _context.Schedules.FirstOrDefaultAsync(s =>
                s.Date.Date == date.Date &&
                s.TimeSlot == timeSlot &&
                (s.RoomId == roomId || s.InstructorId == instructorId || s.CourseId == courseId));

            if (conflictingSchedule != null)
            {
                var allTimeSlots = new List<string>
                {
                    "9:00 AM - 10:30 AM", "10:30 AM - 12:00 PM",
                    "1:00 PM - 2:30 PM", "2:30 PM - 4:00 PM"
                };

                var bookedTimeSlots = await _context.Schedules
                    .Where(s => s.Date.Date == date.Date)
                    .Select(s => s.TimeSlot)
                    .ToListAsync();

                var availableTimeSlots = allTimeSlots.Except(bookedTimeSlots).ToList();

                return (true, "Conflict detected. Please adjust the schedule.", conflictingSchedule, availableTimeSlots);
            }

            return (false, "No conflict detected.", null, null);
        }
    }
}


