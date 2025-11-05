using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityApi.Domain;

namespace UniversityApi.Infrastructure
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(100);
            builder.HasOne(e => e.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(e => e.TeacherId);
        }
    }
}