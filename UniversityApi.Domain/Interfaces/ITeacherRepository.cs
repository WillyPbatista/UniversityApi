namespace UniversityApi.Domain
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
            Task<Teacher> GetByUserIdAsync(string userId);
    }
}