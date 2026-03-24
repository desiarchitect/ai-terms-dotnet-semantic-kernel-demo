using System.Globalization;
using Microsoft.SemanticKernel.Embeddings;

#pragma warning disable SKEXP0001

sealed class EmbeddingsDemo(ITextEmbeddingGenerationService embeddings) : IDemoModule
{
    public string Name => "embeddings";

    public async Task RunAsync()
    {
        ConsoleHelper.PrintSection("2. Embeddings + Cosine Similarity Demo");

        var texts = new[]
        {
            "password reset issue",
            "unable to sign in",
            "this is desi architecture demo code"
        };

        var vectors = await embeddings.GenerateEmbeddingsAsync(texts);
        var a = vectors[0].ToArray();
        var b = vectors[1].ToArray();
        var c = vectors[2].ToArray();

        var sim1 = VectorSearchService.Cosine(a, b);
        var sim2 = VectorSearchService.Cosine(a, c);

        ConsoleHelper.WriteKeyValue("Phrase A", texts[0]);
        ConsoleHelper.WriteKeyValue("Phrase B", texts[1]);
        ConsoleHelper.WriteKeyValue("Phrase C", texts[2]);
        ConsoleHelper.WriteKeyValue("Cosine(A, B)", sim1.ToString("0.000", CultureInfo.InvariantCulture));
        ConsoleHelper.WriteKeyValue("Cosine(A, C)", sim2.ToString("0.000", CultureInfo.InvariantCulture));
        ConsoleHelper.WriteLineAccent("Takeaway: Semantically similar phrases yield higher cosine similarity scores.\n");
    }
}

#pragma warning restore SKEXP0001
