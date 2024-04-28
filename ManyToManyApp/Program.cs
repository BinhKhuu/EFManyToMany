using ManyToMany.Infrastructure.Data;
using ManyToMany.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

var context = new ManyToManyContext();

// Two One-to-Many
context.StudentCourses.ExecuteDelete();
context.Students.ExecuteDelete();
context.Courses.ExecuteDelete();

// Insert new student and new course with navigation property
context.Students.Add(new Student
{
    Id = 1,
    Name = "test",
    // To use navigation property to add to linked table (course and studentcourse) 
    StudentCourses = new List<StudentCourse> { new StudentCourse {  Course = new Course { Id = 1, Name = "test course" } } }
});
context.SaveChanges();

// Insert new course, add new student and update existing student and link to new course
context.Courses.Add(new Course
{
    Id = 2,
    Name = "test course 2",
    StudentCourses = new List<StudentCourse> { new StudentCourse { Student = new Student { Id = 2, Name = "test 2" } }, new StudentCourse { StudentId = 1 } }
});

context.SaveChanges();

var studentCourse = context.StudentCourses.ToList();
var student = context.Students.Include(x => x.StudentCourses).ToList();
var courses = context.Courses.Include(x => x.StudentCourses).ToList();


// Many-to-Many
// Clear db for testing
context.Posts.ExecuteDelete();
context.Tags.ExecuteDelete();

// Insert to one of the many to many tables
context.Posts.Add(new Post
{
    Id = 1,
    // Using the Navigation collection Insert into other many to many table. Tags will have new items specified in the list
    // Make sure they are unique because its inserting
    Tags = new List<Tag> { new Tag { Id = 1 } }
});
context.SaveChanges();

// Create new tag
context.Tags.Add(new Tag
{
    Id = 2
    // dont use the navigation collection it will create a new post
});
context.SaveChanges();

//find the target post
var posts = context.Posts.Include(p => p.Tags).FirstOrDefault();
//find the tag just created
var tag = context.Tags.Where(t => t.Id == 2).FirstOrDefault();
//update its collection
posts?.Tags.Add(tag);
context.SaveChanges();


var allPosts = context.Posts
    .ToList();


foreach (var p in allPosts)
{
    Console.WriteLine(p.Id);
}

var allTags = context.Tags.Include(t => t.Posts).ToList();
foreach (var t in allTags)
{
    Console.WriteLine(t.Id);
}


