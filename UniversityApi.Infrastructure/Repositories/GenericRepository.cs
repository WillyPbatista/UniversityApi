using Microsoft.EntityFrameworkCore;
using UniversityApi.Infrastructure;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly UniversityApiDbContext _context;
    public GenericRepository(UniversityApiDbContext context)
    {   
        _context = context;
    }
    public async Task AddAsync(T entity) => await _context.AddAsync(entity);

    public void Delete(T entity) => _context.Remove(entity);


    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();


    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);


    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();


    public void Update(T entity) => _context.Update(entity);

}