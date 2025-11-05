using Microsoft.EntityFrameworkCore;
using UniversityApi.Domain;

namespace UniversityApi.Infrastructure
{
    public class UniversityApiDbContext : DbContext
    {
        public UniversityApiDbContext(DbContextOptions<UniversityApiDbContext> options) : base(options)
        {

        }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }  
        public DbSet<Enrollment> enrollments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniversityApiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }

}