public interface ICourseService
{
    Task<IEnumerable<CourseDTO>> GetCourses();
    Task<CourseDTO> GetCourse(int Id);
    Task<CourseCreateDTO> CreateCourse(CourseCreateDTO courseCreateDTO);
    Task<CourseDTO> UpdateCourse(CourseDTO courseDTO);
    Task<bool> DeleteCourse(int Id);
}