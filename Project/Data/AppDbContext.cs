using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Project.Entities;
using Project.Entities.Enums;

namespace Project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; private set; }
        public DbSet<Enrollment> Enrollments { get; private set; }
        public DbSet<Instructor> Instructors { get; private set; }
        public DbSet<Student> Students { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<Course>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Instructor>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Enrollment>().HasQueryFilter(c => c.Status != EnrollmentStatus.DROPPED);

            modelBuilder.Entity<Enrollment>().HasIndex(x => new { x.StudentId, x.CourseId })
                .HasFilter("[Status] != 'DROPPED'").IsUnique().HasDatabaseName("IX_Enrollment_StudentCourse_Active");

            modelBuilder.Entity<Instructor>()
                .Ignore(i => i.FullName);

            modelBuilder.Entity<Student>()
                .Ignore(i => i.FullName);
        }
    }
}
