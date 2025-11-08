public interface ICourseService
{
    Task<IEnumerable<CourseDTO>> GetCourses();
    Task<CourseDTO> GetCourse(int Id);
    Task<CourseDTO> CreateCourse(CourseDTO courseDTO);
    Task<CourseDTO> UpdateCourse(CourseDTO courseDTO);
    Task<bool> DeleteCourse(int Id);
}