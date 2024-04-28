using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Infrastructure.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = null!;

    }
}
