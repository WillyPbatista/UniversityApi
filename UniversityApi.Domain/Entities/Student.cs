using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
}
