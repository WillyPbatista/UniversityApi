using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UniversityApi.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UniversityApiDbContext>
    {
        public UniversityApiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversityApiDbContext>();
            optionsBuilder.UseSqlServer("Server=WILLY-PC\\SQLEXPRESS;Database=UniversityDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new UniversityApiDbContext(optionsBuilder.Options);
        }
    }
}
