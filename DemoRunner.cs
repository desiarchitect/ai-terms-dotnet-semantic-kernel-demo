using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;

#pragma warning disable SKEXP0001

sealed class DemoRunner
{
    private readonly IReadOnlyDictionary<string, IDemoModule> _modules;

    private DemoRunner(IReadOnlyDictionary<string, IDemoModule> modules)
        => _modules = modules;

    public static async Task<DemoRunner> CreateAsync()
    {
        var config = AppConfig.Load();
        var kernel  = KernelFactory.Build(config);

        var chat       = kernel.GetRequiredService<IChatCompletionService>();
        var embeddings = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        var store = new KnowledgeStore();
        await store.HydrateEmbeddingsAsync(embeddings);

        var searchService = new VectorSearchService(embeddings);

        IDemoModule[] modules =
        [
            new LlmDemo(chat),
            new EmbeddingsDemo(embeddings),
            new VectorSearchDemo(store, searchService),
            new RagDemo(chat, store, searchService),
            new HallucinationDemo(chat),
        ];

        return new DemoRunner(modules.ToDictionary(m => m.Name));
    }

    public async Task RunAsync(string[] args)
    {
        var command = args.Length == 0 ? "all" : args[0].Trim().ToLowerInvariant();

        if (command == "all")
        {
            await RunAllAsync();
            return;
        }

        if (command is "help" or "--help" or "-h")
        {
            PrintHelp();
            return;
        }

        if (_modules.TryGetValue(command, out var module))
        {
            ConsoleHelper.PrintHeader($"LOCAL OLLAMA + SEMANTIC KERNEL DEMO — {module.Name.ToUpperInvariant()}");
            await module.RunAsync();
        }
        else
        {
            Console.WriteLine($"Unknown command: '{command}'");
            PrintHelp();
        }
    }

    private async Task RunAllAsync()
    {
        ConsoleHelper.PrintHeader("LOCAL OLLAMA + SEMANTIC KERNEL DEMO");

        var orderedModules = _modules.Values.ToList();
        for (var i = 0; i < orderedModules.Count; i++)
        {
            await orderedModules[i].RunAsync();
            if (i < orderedModules.Count - 1)
                ConsoleHelper.Pause();
        }
    }

    private static void PrintHelp()
    {
        ConsoleHelper.PrintHeader("Usage");
        Console.WriteLine("  dotnet run -- all");
        Console.WriteLine("  dotnet run -- llm");
        Console.WriteLine("  dotnet run -- embeddings");
        Console.WriteLine("  dotnet run -- vector");
        Console.WriteLine("  dotnet run -- rag");
        Console.WriteLine("  dotnet run -- hallucination");
        Console.WriteLine();
        Console.WriteLine("Environment variables (or set in .env file):");
        Console.WriteLine("  OLLAMA_ENDPOINT        — default: http://localhost:11434");
        Console.WriteLine("  OLLAMA_CHAT_MODEL      — default: llama3.1");
        Console.WriteLine("  OLLAMA_EMBEDDING_MODEL — default: mxbai-embed-large");
    }
}

#pragma warning restore SKEXP0001
