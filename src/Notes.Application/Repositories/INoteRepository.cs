using Notes.Domain;

namespace Notes.Application.Repositories;

public interface INoteRepository
{
    Task CreateAsync(Note note, CancellationToken cancellationToken = default);
    Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(string noteId, CancellationToken cancellationToken = default);
    Task<Note> GetByIdAsync(string noteId, CancellationToken cancellationToken = default);
    Task<IList<Note>> GetAllAsync(CancellationToken cancellationToken);
}