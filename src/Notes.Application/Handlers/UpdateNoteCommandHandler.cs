using MediatR;
using Notes.Application.Commands;
using Notes.Application.Repositories;
using Notes.Domain;

namespace Notes.Application.Handlers;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Note>
{
    private readonly INoteRepository _noteRepository;

    public UpdateNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Note> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note() { Id = request.Id, Title = request.Title, Text = request.Text};
        await _noteRepository.UpdateAsync(note, cancellationToken);
        return note;
    }
}