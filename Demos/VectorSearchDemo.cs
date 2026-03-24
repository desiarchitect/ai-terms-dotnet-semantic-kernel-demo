sealed class VectorSearchDemo(
    KnowledgeStore store,
    VectorSearchService searchService) : IDemoModule
{
    public string Name => "vector";

    public async Task RunAsync()
    {
        ConsoleHelper.PrintSection("3. Vector Search Demo");

        var query = "user cannot access their account";
        var topMatches = await searchService.RetrieveTopAsync(store.Runbooks, query, topK: 2);

        ConsoleHelper.WriteKeyValue("Query", query);
        foreach (var hit in topMatches)
        {
            ConsoleHelper.WriteKeyValue($"Match {hit.Doc.Id}", $"{hit.Doc.Title} | Score: {hit.Score:0.000}");
            Console.WriteLine($"  {hit.Doc.Text}");
        }

        Console.WriteLine();
        Console.WriteLine("pgvector SQL equivalent (shown as overlay during demo):");
        Console.WriteLine("  SELECT doc_text");
        Console.WriteLine("  FROM support_docs");
        Console.WriteLine("  ORDER BY embedding <=> CAST(@queryEmbedding AS vector)");
        Console.WriteLine("  LIMIT 3;\n");
    }
}
