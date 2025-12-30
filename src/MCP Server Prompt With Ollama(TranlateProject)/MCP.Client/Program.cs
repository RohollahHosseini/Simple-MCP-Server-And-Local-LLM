using Microsoft.Extensions.AI;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using OllamaSharp;

StdioClientTransport ct = new(
    new() 
    {
        Name="TestServer",
        Command= @"D:\Projects\MyProjectsInGitHub\All Project MCP\Simple-MCP-Server-And-Local-LLM\src\MCP Server Prompt With Ollama\MCP.Server\bin\Debug\net9.0\MCP.Server.exe",

    }
    );


await using var mcpClient =await McpClient.CreateAsync( ct,new() 
{
    ClientInfo=new()
    {
        Name = "Test Server",
        Version = "1.0.0"
    }
}).ConfigureAwait(false);



var prompts=await mcpClient.ListPromptsAsync().ConfigureAwait(false);

foreach (var p in prompts)
{
    Console.WriteLine($"{p.Name} => {p.Description}");
}

// Prepare arguments for the prompt
Dictionary<string,object?> arguments= new()
{
    [ "toTranslate"]= "Hello, how are you?"
};

GetPromptResult? prompt= await mcpClient.GetPromptAsync("TranslateToPersianPrompt",arguments).ConfigureAwait(false);

if(prompt is null)
{
    Console.WriteLine("Error");
}
else
{
    List<ChatMessage> messageForLLM = [];

    foreach (var msg in prompt.Messages)
    {
        ContentBlock cb = msg.Content;
        var text=((TextContentBlock)cb).Text;
        Console.WriteLine($"Role: {msg.Role} | Text: {text}");

        //Add to message for LLM
        messageForLLM.Add(AIContentExtensions.ToChatMessage(msg));
    }

    // Now, send messageForLLM to your local LLM using Ollama client
    string ollamaUrl = "http://localhost:11434";
    string modelId = "gemma3:4b";

    IChatClient ollamaClient = new OllamaApiClient(ollamaUrl, modelId);

    // Build chat client
    var chatClient =ChatClientBuilderChatClientExtensions.AsBuilder(ollamaClient).Build();

    await foreach (var response in chatClient.GetStreamingResponseAsync(messageForLLM))
    {
        Console.Write(response);
    }

    Console.ReadKey();

}

