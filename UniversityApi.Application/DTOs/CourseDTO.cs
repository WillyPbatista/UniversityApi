using UniversityApi.Domain;

public class CourseDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Credits { get; set; }
    public TeacherDTO? Teacher { get; set; }
}