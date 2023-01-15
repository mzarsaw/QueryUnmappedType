using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dapper;
using Microsoft.EntityFrameworkCore;
using QueryUnmappedTypes.Db;
using QueryUnmappedTypes.Extensions;
using QueryUnmappedTypes.Helper;
using QueryUnmappedTypes.ViewModels;
using System.Data;

namespace QueryUnmappedTypes
{
    [MemoryDiagnoser]
    public class BenchmarkQuery
    {
        private readonly SampleDbContext dbContext = new SampleDbContext(new DbContextOptionsBuilder().UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=sampleDb;Integrated Security=SSPI").Options);
        private IDbConnection _dbConnection = default!;
        private const string sqlQuery = @"SELECT        Students.Firstname+' '+ Students.Lastname as StudentName, Lessons.Title as LessonTitle, StudentLessons.Date
FROM            Lessons INNER JOIN
                         StudentLessons ON Lessons.Id = StudentLessons.LessonId INNER JOIN
                         Students ON StudentLessons.StudentId = Students.Id";
        [GlobalSetup]
        public async Task Setup()
        {
            await new DataInitializer(dbContext).Go();
            _dbConnection = dbContext.Database.GetDbConnection();
        }
        [Benchmark]
        public async Task DapperQuery() => (await _dbConnection.QueryAsync<UnmappedStudentLesson>(sqlQuery)).ToList();

        [Benchmark]
        public async Task MappedQuery() =>
            await dbContext.StudentLessons.Select(x => new UnmappedStudentLesson
            {
                Date = x.Date,
                LessonTitle = x.Lesson.Title,
                StudentName = $"{x.Student.Firstname} {x.Student.Lastname}"
            }).ToListAsync();

        [Benchmark]
        public async Task UnMappedQuery() =>
            await dbContext.FromSqlRawAsync<UnmappedStudentLesson>(sqlQuery);

    }
}
