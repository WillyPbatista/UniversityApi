using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Domain
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        public DateTime HireDate { get; set; }
        public ICollection<Course>?  Courses { get; set; }
    }
}