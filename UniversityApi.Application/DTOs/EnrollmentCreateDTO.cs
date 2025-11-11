using System.ComponentModel.DataAnnotations;

public class EnrollmentCreateDTO
{
    [Required(ErrorMessage = "Student id is required")]
    public int StudentId { get; set; }
    [Required(ErrorMessage = "Corse id is required")]
    public int CourseId { get; set; }
    public decimal? Grade { get; set; }
}