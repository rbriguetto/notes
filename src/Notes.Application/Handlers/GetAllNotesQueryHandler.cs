using MediatR;
using Notes.Application.Queries;
using Notes.Application.Repositories;
using Notes.Domain;

namespace Notes.Application.Handlers;

public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IList<Note>>
{
    private readonly INoteRepository _noteRepository;

    public GetAllNotesQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public Task<IList<Note>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
    {
        return _noteRepository.GetAllAsync(cancellationToken);
    }
}