using MediatR;
using Notes.Application.Commands;
using Notes.Application.Repositories;

namespace Notes.Application.Handlers;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly INoteRepository _noteRepository;

    public DeleteNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        await _noteRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}