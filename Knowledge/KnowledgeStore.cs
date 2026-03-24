using Microsoft.SemanticKernel.Embeddings;

#pragma warning disable SKEXP0001

sealed class KnowledgeStore
{
    public List<KnowledgeDoc> Runbooks { get; } =
    [
        new("rb-1", "Login Runbook",    "If users cannot log in after a password reset, verify token expiry and identity provider sync."),
        new("rb-2", "Migration Runbook","If a deployment fails during migration, check for schema version mismatch and execute the rollback plan."),
        new("rb-3", "MFA Incident",     "If users are locked out after an MFA update, verify device enrollment sync and retry policy."),
    ];

    public List<KnowledgeDoc> Policies { get; } =
    [
        new("p-1", "Leave Policy",    "Employees can carry forward up to 10 unused paid leaves to the next calendar year. Casual leaves do not carry forward."),
        new("p-2", "WFH Policy",      "Employees may work from home up to 2 days per week with manager approval."),
        new("p-3", "Incident Policy", "Severity-1 incidents must be acknowledged within 15 minutes and escalated to the on-call manager."),
    ];

    public async Task HydrateEmbeddingsAsync(ITextEmbeddingGenerationService embeddings)
    {
        foreach (var doc in Runbooks.Concat(Policies))
            doc.Vector = (await embeddings.GenerateEmbeddingAsync(doc.Text)).ToArray();
    }
}

#pragma warning restore SKEXP0001
