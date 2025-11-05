using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApi.Domain;

namespace UniversityApi.Infrastructure
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
            builder.Property(x => x.EnrollmentDate)
            .HasColumnType("date");
        }
    }
}