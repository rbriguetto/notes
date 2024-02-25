using Microsoft.EntityFrameworkCore;
using Notes.Application.Repositories;
using Notes.Domain;
using Notes.Infraestructure.Exceptions;

namespace Notes.Infraestructure.Infraestructure;

public class EfNoteRepository : INoteRepository
{
    private NotesDbContext _dbContext;

    public EfNoteRepository(NotesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        var record = NoteRecord.FromEntity(note);
        await _dbContext.Notes.AddAsync(record, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(string noteId, CancellationToken cancellationToken = default)
    {
        var record = await _dbContext.Notes.FindAsync(noteId, cancellationToken);
        if (record == null)
        {
            throw new NoteNotFoundException();
        }
        _dbContext.Notes.Remove(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Note>> GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _dbContext.Notes
            .AsNoTracking()
            .ToListAsync(cancellationToken))
            .Select(record => record.ToEntity())
            .ToList();
    }

    public async Task<Note> GetByIdAsync(string noteId, CancellationToken cancellationToken = default)
    {
        var record = await _dbContext.Notes.FindAsync(noteId, cancellationToken);
        if (record == null)
        {
            throw new NoteNotFoundException();
        }
        return record.ToEntity();
    }

    public async Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        var record = await _dbContext.Notes.FindAsync(note.Id, cancellationToken);
        if (record == null)
        {
            throw new NoteNotFoundException();
        }
        record.Title = note.Title;
        record.Text = note.Text;
        _dbContext.Update(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}