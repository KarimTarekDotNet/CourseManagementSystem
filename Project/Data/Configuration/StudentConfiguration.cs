using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities;

namespace Project.Data.Configuration
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(i => i.FirstName).HasMaxLength(50).IsRequired();

            builder.Property(i => i.LastName).HasMaxLength(50).IsRequired();

            builder.Property(s => s.College).HasMaxLength(100);

            builder.Property(s => s.Email).HasMaxLength(100).IsRequired();
            builder.HasIndex(s => s.Email).IsUnique();

            builder.Property(s => s.PhoneNumber).HasMaxLength(15).IsRequired();
            builder.HasIndex(s => s.PhoneNumber).IsUnique();

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();


            builder.HasMany(x => x.Enrollments)
                   .WithOne(e => e.Student)
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.ToTable("Students");
        }
    }
}
