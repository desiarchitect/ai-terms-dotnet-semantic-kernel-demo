using Microsoft.SemanticKernel.ChatCompletion;

sealed class RagDemo(
    IChatCompletionService chat,
    KnowledgeStore store,
    VectorSearchService searchService) : IDemoModule
{
    public string Name => "rag";

    public async Task RunAsync()
    {
        ConsoleHelper.PrintSection("4. RAG (Retrieval-Augmented Generation) Demo");

        var question = "Can unused leaves be carried forward to the next calendar year?";
        var retrieved = await searchService.RetrieveTopAsync(store.Policies, question, topK: 2);
        var context = string.Join("\n", retrieved.Select(x => x.Doc.Text));

        var history = new ChatHistory();
        history.AddSystemMessage($"""
            Answer only from the provided context.
            If the answer is not present in the context, say you don't know.

            Context:
            {context}
            """);
        history.AddUserMessage(question);

        var response = await chat.GetChatMessageContentAsync(history);

        ConsoleHelper.WriteKeyValue("Question", question);
        Console.WriteLine("Retrieved Context:");
        foreach (var hit in retrieved)
        {
            Console.WriteLine($"  - {hit.Doc.Title} (score: {hit.Score:0.000})");
            Console.WriteLine($"    {hit.Doc.Text}");
        }
        ConsoleHelper.WriteKeyValue("Grounded Answer", response.Content ?? "<empty>");
        ConsoleHelper.WriteLineAccent("Takeaway: RAG = retrieve relevant context first, then generate an answer grounded in that context.\n");
    }
}
