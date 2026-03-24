sealed class AppConfig
{
    public required string OllamaEndpoint { get; init; }
    public required string ChatModel { get; init; }
    public required string EmbeddingModel { get; init; }

    public static AppConfig Load()
    {
        LoadDotEnv();
        return new AppConfig
        {
            OllamaEndpoint  = Environment.GetEnvironmentVariable("OLLAMA_ENDPOINT")        ?? "http://localhost:11434",
            ChatModel       = Environment.GetEnvironmentVariable("OLLAMA_CHAT_MODEL")      ?? "llama3.1",
            EmbeddingModel  = Environment.GetEnvironmentVariable("OLLAMA_EMBEDDING_MODEL") ?? "mxbai-embed-large"
        };
    }

    private static void LoadDotEnv(string path = ".env")
    {
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadLines(path))
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0 || trimmed.StartsWith('#')) continue;

            var separatorIndex = trimmed.IndexOf('=');
            if (separatorIndex <= 0) continue;

            var key   = trimmed[..separatorIndex].Trim();
            var value = trimmed[(separatorIndex + 1)..].Trim();

            if (Environment.GetEnvironmentVariable(key) is null)
                Environment.SetEnvironmentVariable(key, value);
        }
    }
}
