using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UniversityApi.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UniversityApiDbContext>
    {
        public UniversityApiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversityApiDbContext>();
            optionsBuilder.UseSqlServer("UniversityConnection");

            return new UniversityApiDbContext(optionsBuilder.Options);
        }
    }
}
