namespace QueryUnmappedTypes.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<StudentLesson> Students { get; set; } = default!;
    }
}
