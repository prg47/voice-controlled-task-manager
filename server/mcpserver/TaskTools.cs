using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace mcpserver;

[McpServerToolType]
public static class TaskTools
{
    [McpServerTool, Description("Create a new task with optional description")]
    public static async Task<TaskItem> CreateTaskAsync(string title, string? description = null)
    {
        await using var db = new AppDbContext();
        var task = new TaskItem
        {
            Title = title,
            Description = description ?? string.Empty,
            CurrentStatus = Status.Ongoing
        };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return task;
    }

    [McpServerTool, Description("Get a task and its subtasks by ID")]
    public static async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        await using var db = new AppDbContext();
        return await db.Tasks.Include(t => t.Subtasks)
                             .FirstOrDefaultAsync(t => t.Id == id);
    }

    [McpServerTool, Description("List all tasks with subtasks")]
    public static async Task<List<TaskItem>> ListTasksAsync()
    {
        await using var db = new AppDbContext();
        return await db.Tasks.Include(t => t.Subtasks).ToListAsync();
    }

    [McpServerTool, Description("Delete a task and its subtasks")]
    public static async Task<bool> DeleteTaskAsync(int id)
    {
        await using var db = new AppDbContext();
        var task = await db.Tasks.Include(t => t.Subtasks)
                                 .FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return false;

        db.Subtasks.RemoveRange(task.Subtasks);
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    [McpServerTool, Description("Mark a task as completed")]
    public static async Task<bool> CompleteTaskAsync(int taskId)
    {
        await using var db = new AppDbContext();
        var task = await db.Tasks.FindAsync(taskId);
        if (task == null) return false;

        task.CurrentStatus = Status.Completed;
        await db.SaveChangesAsync();
        return true;
    }

}
