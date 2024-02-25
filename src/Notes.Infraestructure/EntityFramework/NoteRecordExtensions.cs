using Notes.Domain;

namespace Notes.Infraestructure.Infraestructure;

public static class NoteRecordExtensions
{
        public static Note ToEntity(this NoteRecord record) 
            => new Note(record.Id, record.Title, record.Text);
}