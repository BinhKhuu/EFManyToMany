using System;
using System.Collections.Generic;
using ManyToMany.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ManyToMany.Infrastructure.Data;

public partial class ManyToManyContext : DbContext
{
    public ManyToManyContext()
    {
    }

    public ManyToManyContext(DbContextOptions<ManyToManyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFManyToMany;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


    private void buildTwo_OneToMany(ModelBuilder modelBuilder)
    {
        // two one-to-many 
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(s => s.Id).ValueGeneratedNever();
            //EF automatically maps this, will cause issues if you map it yourself
            //entity.HasMany<StudentCourse>().WithOne(s => s.Student).HasForeignKey("StudentId");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(c => c.Id).ValueGeneratedNever();
            //EF automatically maps this, will cause issues if you map it yourself
            //entity.HasMany<StudentCourse>().WithOne(c => c.Course).HasForeignKey("CourseId");
        });

        modelBuilder.Entity<StudentCourse>().HasKey(x => new { x.StudentId, x.CourseId });
    }

    private void buildManyToMany(ModelBuilder modelBuilder)
    {
        // Build the entity and relationships - Let EF do this for you
        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();


            // Specifying the Modeling in Using entity does work if the column names for the foreign keys don't follow naming conventions
            // below tries to remap the the column names but doesnt work.
            //entity
            //    .HasMany(d => d.Tags)
            //    .WithMany(p => p.Posts)
            //    .UsingEntity<PostsToTagsJoinTable>(
            //        l => l.HasOne<Tag>().WithMany().HasForeignKey(e => e.TagsId),
            //        r => r.HasOne<Post>().WithMany().HasForeignKey(e => e.PostsId),
            //        j => {
            //            j.Property("PostId").HasColumnName("PostsId");
            //            j.Property("TagId").HasColumnName("TagsId");
            //        });


            // Using entity but specify the table name works when remapping the foreignkey
            entity
                .HasMany(d => d.Tags).WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostsToTagsJoinTable",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagsId"),
                    l => l.HasOne<Post>().WithMany().HasForeignKey("PostsId"),
                    j =>
                    {
                        j.HasKey("PostsId", "TagsId");
                        j.ToTable("PostsToTagsJoinTable");
                    });

            // alternative way 
            //entity.HasMany(e => e.Tags)
            //    .WithMany(e => e.Posts)
            //    .UsingEntity(
            //        "PostsToTagsJoinTable",
            //        l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagsId").HasPrincipalKey(nameof(Tag.Id)),
            //        r => r.HasOne(typeof(Post)).WithMany().HasForeignKey("PostsId").HasPrincipalKey(nameof(Post.Id)),
            //        j => j.HasKey("PostsId", "TagsId")
            //    );
        });
        // Entity already linked in Post don't need to specify it for the Tag.
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });


    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed data
        modelBuilder.Entity<Post>().HasData(new Post { Id = 1 });
        modelBuilder.Entity<Tag>().HasData(new Tag { Id = 1 });
        modelBuilder.Entity<Tag>().HasData(new Tag { Id = 2 });

        // Seed the Joining table with UsingEntity
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(p => p.Posts)
            .UsingEntity(j => j
                .HasData(new[]
                    {
                        new { PostsId = 1, TagsId = 1 },
                        new { PostsId = 1, TagsId = 2 }
                    }
                ));

        // Run Add-Migration <YourMigrationName>
        // Update-Database
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        buildTwo_OneToMany(modelBuilder);
        buildManyToMany(modelBuilder);
        SeedData(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
