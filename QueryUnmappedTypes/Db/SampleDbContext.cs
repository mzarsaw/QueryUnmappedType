using Microsoft.EntityFrameworkCore;
using QueryUnmappedTypes.Models;

namespace QueryUnmappedTypes.Db
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Lesson> Lessons { get; set; } = default!;
        public DbSet<StudentLesson> StudentLessons { get; set; } = default!;
    }
}
