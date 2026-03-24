sealed class KnowledgeDoc(string id, string title, string text)
{
    public string Id { get; } = id;
    public string Title { get; } = title;
    public string Text { get; } = text;
    public float[] Vector { get; set; } = [];
}
