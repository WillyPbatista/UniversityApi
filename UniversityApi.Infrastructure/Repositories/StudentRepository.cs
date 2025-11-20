using UniversityApi.Domain;

namespace UniversityApi.Infrastructure
{

    public class StudentRepository :  GenericRepository<Student>, IStudentRepository
    {
        private readonly UniversityApiDbContext _context;
        public StudentRepository(UniversityApiDbContext context)   : base(context)
        {
            _context = context;
        }

        public async Task<Student> GetByUserIdAsync(string userId)
        {
            return  _context.Students.FirstOrDefault(x => x.UserId == userId);

        }

    }

}