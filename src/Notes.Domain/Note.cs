namespace Notes.Domain;

public readonly record struct Note
{
    public static Note Empty = new Note(string.Empty, string.Empty, string.Empty);

    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Title { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;

    public Note(string id, string title, string text) => (Id, Title, Text) = (id, title, text);

}
