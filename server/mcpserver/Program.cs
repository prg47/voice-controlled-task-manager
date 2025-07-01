using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using mcpserver;

var builder = Host.CreateApplicationBuilder(args);

using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();  
}

builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport() 
    .WithToolsFromAssembly(typeof(TaskTools).Assembly);

var app = builder.Build();
await app.RunAsync();  
