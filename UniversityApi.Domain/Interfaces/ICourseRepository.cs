using UniversityApi.Domain;

public interface ICourseRepository
{
    Task<Course> GetById(int id);
    Task<List<Course>> GetAll();
}
