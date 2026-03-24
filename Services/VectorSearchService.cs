using Microsoft.SemanticKernel.Embeddings;

#pragma warning disable SKEXP0001

sealed class VectorSearchService(ITextEmbeddingGenerationService embeddings)
{
    public async Task<List<SearchHit>> RetrieveTopAsync(
        List<KnowledgeDoc> docs, string query, int topK)
    {
        var queryVector = (await embeddings.GenerateEmbeddingAsync(query)).ToArray();

        return docs
            .Select(doc => new SearchHit(doc, Cosine(queryVector, doc.Vector)))
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .ToList();
    }

    public static double Cosine(float[] a, float[] b)
    {
        double dot  = 0;
        double magA = 0;
        double magB = 0;

        for (var i = 0; i < a.Length; i++)
        {
            dot  += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        if (magA == 0 || magB == 0) return 0;
        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }
}

#pragma warning restore SKEXP0001
