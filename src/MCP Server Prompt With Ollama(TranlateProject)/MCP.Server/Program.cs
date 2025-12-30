using MCP.Server.Prompts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var buidler = Host.CreateApplicationBuilder(args);

buidler.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithPrompts<TranslatePrompt>();

var app=buidler.Build();

await app.RunAsync();