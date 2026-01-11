using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Entities;
using System.Reflection.Emit;

namespace Project.Data.Configuration
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(i => i.LastName).HasMaxLength(50).IsRequired();

            builder.Property(i => i.Department).HasMaxLength(100);

            builder.Property(i => i.Email).HasMaxLength(100).IsRequired();
            builder.HasIndex(i => i.Email).IsUnique();

            builder.Property(i => i.PhoneNumber).HasMaxLength(15).IsRequired();
            builder.HasIndex(i => i.PhoneNumber).IsUnique();

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasMany(i => i.Courses)
                   .WithOne(c => c.Instructor)
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Instructors");
        }
    }
}
