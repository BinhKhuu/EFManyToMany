using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Infrastructure.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
