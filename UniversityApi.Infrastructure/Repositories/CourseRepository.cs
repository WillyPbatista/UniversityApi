using Microsoft.EntityFrameworkCore;
using UniversityApi.Domain;
using UniversityApi.Infrastructure;

public class CourseRepository : ICourseRepository
{
    private readonly UniversityApiDbContext _context;

    public CourseRepository(UniversityApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAll()
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .ToListAsync();
    }

    public async Task<Course> GetById(int id)
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}