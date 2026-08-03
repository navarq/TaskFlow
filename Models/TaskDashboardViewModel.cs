namespace TaskFlow.Models;

public class TaskDashboardViewModel
{
    public List<TaskItem> Tasks { get; set; } = new();
    public int TotalTasks { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int OverdueCount { get; set; }
    public string? SearchTerm { get; set; }
    public TaskState? SelectedStatus { get; set; }
}
