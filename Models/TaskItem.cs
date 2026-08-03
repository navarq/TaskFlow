using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [Required]
    public TaskState Status { get; set; } = TaskState.Backlog;

    [Display(Name = "Assignee")]
    [StringLength(80)]
    public string? Assignee { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Due date")]
    public DateTime? DueDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum TaskState
{
    Backlog,
    InProgress,
    Review,
    Completed
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}
