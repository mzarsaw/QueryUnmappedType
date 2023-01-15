using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using QueryUnmappedTypes;
using QueryUnmappedTypes.Db;
using QueryUnmappedTypes.Extensions;
using QueryUnmappedTypes.Helper;
using QueryUnmappedTypes.ViewModels;

var dbContext = new SampleDbContext(new DbContextOptionsBuilder().UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=sampleDb;Integrated Security=SSPI").Options);
await new DataInitializer(dbContext).Go();

Console.WriteLine("------------------------------------------------------");
Console.WriteLine("use mapped type");
var mappedQuery = dbContext.StudentLessons.Select(x => new UnmappedStudentLesson
{
    Date = x.Date,
    LessonTitle = x.Lesson.Title,
    StudentName = $"{x.Student.Firstname} {x.Student.Lastname}"
});
foreach (var item in mappedQuery)
{
    Console.WriteLine(item.ToJson());
}

Console.WriteLine("------------------------------------------------------");
Console.WriteLine("use unmapped type");
var unmappedQuery = await dbContext.FromSqlRawAsync<UnmappedStudentLesson>(@"SELECT        Students.Firstname+' '+ Students.Lastname as StudentName, Lessons.Title as LessonTitle, StudentLessons.Date
FROM            Lessons INNER JOIN
                         StudentLessons ON Lessons.Id = StudentLessons.LessonId INNER JOIN
                         Students ON StudentLessons.StudentId = Students.Id");

foreach (var item in unmappedQuery)
{
    Console.WriteLine(item.ToJson());
}
var summary = BenchmarkRunner.Run<BenchmarkQuery>();