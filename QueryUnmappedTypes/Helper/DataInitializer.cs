using Microsoft.EntityFrameworkCore;
using QueryUnmappedTypes.Models;

namespace QueryUnmappedTypes.Helper
{
    public class DataInitializer
    {
        private readonly Dictionary<string, object[]> _testData = new()
        {
            {
                nameof(Student),
                new[]
                {
                    new Student{ Firstname="mazhar", Lastname="zarsaw" },
                    new Student{ Firstname="John", Lastname="Doe" },

                }
            },
            {
                nameof(Lesson),
                new[]
                {
                    new Lesson{ Title="Lesson 1"},
                    new Lesson{ Title="Lesson 2" },

                }
            },
            {
                nameof(StudentLesson),
                new[]
                {
                    new StudentLesson{ StudentId=1,LessonId=1,Date=new DateTime(2023,01,10)},
                    new StudentLesson{ StudentId=1,LessonId=2,Date=new DateTime(2023,01,10)},
                    new StudentLesson{ StudentId=2,LessonId=1,Date=new DateTime(2023,01,10)},
                }
            }
        };

        public DbContext _dbContext { get; }

        public DataInitializer(
            DbContext dbContext,
            Dictionary<string, object[]>? testData = null)
        {
            _dbContext = dbContext;
            _testData = testData ?? _testData;
        }

        public async Task Go()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();

            foreach (var item in _testData)
            {
                await _dbContext.AddRangeAsync(item.Value);
                await _dbContext.SaveChangesAsync();

            }

        }
    }
}
