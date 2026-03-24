using Microsoft.SemanticKernel;

#pragma warning disable SKEXP0070

static class KernelFactory
{
    public static Kernel Build(AppConfig config)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion(
            modelId:  config.ChatModel,
            endpoint: new Uri(config.OllamaEndpoint));

        builder.AddOllamaTextEmbeddingGeneration(
            modelId:  config.EmbeddingModel,
            endpoint: new Uri(config.OllamaEndpoint));

        builder.Plugins.AddFromType<TicketPlugin>("tickets");
        builder.Plugins.AddFromType<PolicyPlugin>("policy");

        return builder.Build();
    }
}

#pragma warning restore SKEXP0070
