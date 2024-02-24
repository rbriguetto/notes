using MediatR;
using Notes.Domain;

namespace Notes.Application.Queries;

public class GetAllNotesQuery : IRequest<IList<Note>>
{

}
