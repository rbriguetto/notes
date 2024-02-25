using Notes.Domain;

namespace Notes.Infraestructure.Infraestructure;

public class NoteRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public static NoteRecord FromEntity(Note entity) => 
        new NoteRecord() { Id = entity.Id, Title = entity.Title, Text = entity.Text } ;

}