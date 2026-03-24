using Microsoft.SemanticKernel.ChatCompletion;

sealed class HallucinationDemo(IChatCompletionService chat) : IDemoModule
{
    public string Name => "hallucination";

    private static readonly HashSet<string> AllowedLeaveTypes =
        new(StringComparer.OrdinalIgnoreCase) { "Casual", "Sick", "Paid" };

    public async Task RunAsync()
    {
        ConsoleHelper.PrintSection("5. Hallucination + Validation Guardrails Demo");

        var history = new ChatHistory();
        history.AddSystemMessage("Return exactly one leave type name and nothing else.");
        history.AddUserMessage("Suggest a premium-sounding leave type for executives.");

        var response = await chat.GetChatMessageContentAsync(history);
        var modelOutput = (response.Content ?? string.Empty).Trim().Trim('"');
        var isValid = AllowedLeaveTypes.Contains(modelOutput);

        ConsoleHelper.WriteKeyValue("Raw Model Output",    string.IsNullOrWhiteSpace(modelOutput) ? "<empty>" : modelOutput);
        ConsoleHelper.WriteKeyValue("Validation Result",   isValid ? "Accepted" : "Rejected — not in allowed list");
        Console.WriteLine("Allowed values:");
        foreach (var item in AllowedLeaveTypes)
            Console.WriteLine($"  - {item}");

        ConsoleHelper.WriteLineAccent("Takeaway: even a convincing model response can be caught and rejected by a business-layer validation guard.\n");
    }
}
