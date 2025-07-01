using Microsoft.EntityFrameworkCore;

namespace mcpserver;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<SubtaskItem> Subtasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder o)
        => o.UseSqlite("Data Source=task.db");
}
