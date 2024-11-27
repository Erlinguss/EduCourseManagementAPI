using System.ComponentModel.DataAnnotations;

namespace EducationCourseManagement.DTOs
{
    public class StudentDTO
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Email { get; set; }
    }
}