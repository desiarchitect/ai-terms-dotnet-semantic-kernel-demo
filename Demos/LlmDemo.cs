using Microsoft.SemanticKernel.ChatCompletion;

sealed class LlmDemo(IChatCompletionService chat) : IDemoModule
{
    public string Name => "llm";

    public async Task RunAsync()
    {
        ConsoleHelper.PrintSection("1. LLM Demo");

        var history = new ChatHistory();
        history.AddSystemMessage("You explain backend concepts to experienced .NET developers concisely. Answer in 2 short lines.");
        history.AddUserMessage("Explain Redis cache.");

        var response = await chat.GetChatMessageContentAsync(history);

        ConsoleHelper.WriteKeyValue("Prompt", "Explain Redis cache.");
        ConsoleHelper.WriteKeyValue("Model Output", response.Content ?? "<empty>");
        ConsoleHelper.WriteLineAccent("Takeaway: LLM = text in, text out — powered by a locally served Ollama model.\n");
    }
}
