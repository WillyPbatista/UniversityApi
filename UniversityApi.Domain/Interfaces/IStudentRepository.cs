namespace UniversityApi.Domain
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
            Task<Student> GetByUserIdAsync(string userId);
    }
}