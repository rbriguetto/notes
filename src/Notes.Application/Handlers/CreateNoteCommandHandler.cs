using MediatR;
using Notes.Application.Commands;
using Notes.Application.Repositories;
using Notes.Domain;

namespace Notes.Application.Handlers;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Note>
{
    private readonly INoteRepository _noteRepository;

    public CreateNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Note> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note() { Id = Guid.NewGuid().ToString(), Title = request.Title, Body = request.Text };
        await _noteRepository.CreateAsync(note, cancellationToken);
        return note;
    }
}