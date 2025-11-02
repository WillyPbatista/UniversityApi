using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApi.Domain
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Credits { get; set; }
        [ForeignKey("TeacherId")]
        public int TeacherId { get; set; }        
    }
}