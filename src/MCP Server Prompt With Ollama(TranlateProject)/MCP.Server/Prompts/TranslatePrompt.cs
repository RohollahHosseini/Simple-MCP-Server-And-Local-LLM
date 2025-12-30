using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCP.Server.Prompts
{
    [McpServerPromptType]
    public class TranslatePrompt
    {
        [McpServerPrompt(Name = "TranslateToPersianPrompt"), Description("Prompt to translate to persian")]
        public static IEnumerable<ChatMessage> TranslateToPersianPrompt([Description("English text to translate Perian")] string toTranslate)
        {
            return [new ChatMessage(ChatRole.User, $"Translate the following text from English to Persian :{toTranslate}")];
        }


    }
}
