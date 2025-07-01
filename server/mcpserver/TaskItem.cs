using System;

namespace mcpserver;

public enum Status
{
    Completed,
    Ongoing,
    Overdue
}

public class TaskItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    
    public Status CurrentStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property for subtasks
    public List<SubtaskItem> Subtasks { get; set; } = new();

    // Computed Progress (0–100%)
    public int Progress
    {
        get
        {
            if (Subtasks == null || Subtasks.Count == 0)
                return CurrentStatus == Status.Completed ? 100 : 0;

            var completed = Subtasks.Count(s => s.CurrentStatus == Status.Completed);
            return (int)Math.Round((double)completed / Subtasks.Count * 100);
        }
    }
}

public class SubtaskItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public Status CurrentStatus
    { get; set; }

    // Foreign key
    public int TaskItemId { get; set; }
    public required TaskItem Task { get; set; }
}
