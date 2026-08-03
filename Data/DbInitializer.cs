using TaskFlow.Models;

namespace TaskFlow.Data;

public static class DbInitializer
{
    public static void Initialize(TaskFlowDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Tasks.Any())
        {
            return;
        }

        var today = DateTime.UtcNow;

        context.Tasks.AddRange(
            new TaskItem
            {
                Title = "Design launch plan",
                Description = "Finalize the release timeline, milestones, and stakeholder communications for the next feature drop.",
                Status = TaskState.InProgress,
                Priority = TaskPriority.High,
                Assignee = "Maya",
                DueDate = today.AddDays(2),
                CreatedAt = today.AddDays(-3),
                UpdatedAt = today.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Prepare customer onboarding checklist",
                Description = "Document the sequence for new customer setup, team training, and success milestones.",
                Status = TaskState.Backlog,
                Priority = TaskPriority.Medium,
                Assignee = "Lucas",
                DueDate = today.AddDays(5),
                CreatedAt = today.AddDays(-6),
                UpdatedAt = today.AddDays(-2)
            },
            new TaskItem
            {
                Title = "Review analytics dashboard",
                Description = "Validate the recent metrics update and confirm that the conversion funnel is tracking correctly.",
                Status = TaskState.Review,
                Priority = TaskPriority.Critical,
                Assignee = "Avery",
                DueDate = today.AddDays(-1),
                CreatedAt = today.AddDays(-8),
                UpdatedAt = today.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Clean up sprint backlog",
                Description = "Archive completed tasks, update labels, and remove duplicate stories no longer scheduled for delivery.",
                Status = TaskState.Completed,
                Priority = TaskPriority.Low,
                Assignee = "Olivia",
                DueDate = today.AddDays(-3),
                CreatedAt = today.AddDays(-10),
                UpdatedAt = today.AddDays(-4)
            }
        );

        context.SaveChanges();
    }
}
