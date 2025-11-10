using System.ComponentModel.DataAnnotations;

public class StudentCreateDTO
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    public DateTime EnrollmentDate { get; set; }
}