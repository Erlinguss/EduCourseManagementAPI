using Microsoft.EntityFrameworkCore;
using EducationCourseManagement.Data;
using EducationCourseManagement.DTOs;
using EduCourseManagementAPI.Interfaces;
using EducationCourseManagement.Models;

namespace EducationCourseManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();

            return students.Select(s => new StudentDTO
            {
                StudentId = s.StudentId,
                Name = s.Name,
                Email = s.Email
            });
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return null;

            return new StudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email
            };
        }

        public async Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO)
        {
            var student = new Student
            {
                Name = studentDTO.Name,
                Email = studentDTO.Email
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            studentDTO.StudentId = student.StudentId;
            return studentDTO;
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDTO studentDTO)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            student.Name = studentDTO.Name;
            student.Email = studentDTO.Email;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
