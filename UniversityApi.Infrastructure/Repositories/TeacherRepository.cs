using UniversityApi.Domain;

namespace UniversityApi.Infrastructure
{

    public class TeacherRepository :  GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly UniversityApiDbContext _context;
        public TeacherRepository(UniversityApiDbContext context)   : base(context)
        {
            _context = context;
        }

        public async Task<Teacher> GetByUserIdAsync(string userId)
        {
            return  _context.Teachers.FirstOrDefault(x => x.UserId == userId);

        }

    }

}