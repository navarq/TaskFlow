using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Data;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.DueDate);
            entity.Property(t => t.Title).HasMaxLength(120);
            entity.Property(t => t.Assignee).HasMaxLength(80);
        });

        base.OnModelCreating(modelBuilder);
    }
}
