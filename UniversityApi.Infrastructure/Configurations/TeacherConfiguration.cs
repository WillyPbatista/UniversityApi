using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApi.Domain;

namespace UniversityApi.Infrastructure
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
            .HasMaxLength(100);
            builder.Property(x => x.HireDate)
            .HasColumnType("date");
        }
    }
}
