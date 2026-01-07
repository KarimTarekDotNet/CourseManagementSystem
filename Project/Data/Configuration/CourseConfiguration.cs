using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities;

namespace Project.Data.Configuration
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

            builder.Property(c => c.Description).HasMaxLength(100).IsRequired(false);
            builder.Property(c => c.TotalHours).IsRequired();
            builder.Property(c => c.SessionDuration).IsRequired();
            builder.Property(c => c.Capacity).IsRequired();
            builder.Property(c => c.Level)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasMany(c => c.Enrollments)
                   .WithOne(e => e.Course)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Instructor)
                    .WithMany(i => i.Courses)
                    .HasForeignKey(e => e.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            builder.ToTable("Courses");
        }
    }
}
