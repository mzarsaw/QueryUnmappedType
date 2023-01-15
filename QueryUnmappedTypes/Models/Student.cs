namespace QueryUnmappedTypes.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public ICollection<StudentLesson> Lessons { get; set; } = default!;
    }
}
