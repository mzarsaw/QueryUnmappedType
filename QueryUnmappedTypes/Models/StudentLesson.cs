using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryUnmappedTypes.Models
{
    public class StudentLesson
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public DateTime Date { get; set; }
        public Student Student { get; set; } = default!;
        public Lesson Lesson { get; set; } = default!;
    }
}
