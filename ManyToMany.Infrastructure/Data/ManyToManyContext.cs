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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFManyToMany;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Build the entity and relationships - Let EF do this for you
        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasMany(d => d.Tags).WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostsToTagsJoinTable",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagsId"),
                    l => l.HasOne<Post>().WithMany().HasForeignKey("PostsId"),
                    j =>
                    {
                        j.HasKey("PostsId", "TagsId");
                        j.ToTable("PostsToTagsJoinTable");
                    });
        });
        // Entity already linked in Post don't need to specify it for the Tag.
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
