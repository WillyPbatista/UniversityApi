using System.ComponentModel.DataAnnotations;

public class CourseCreateDTO
{
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }
    public string? Credits { get; set; }
    public string TeacherId { get; set; }
}