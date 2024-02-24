using MediatR;
using Notes.Domain;

namespace Notes.Application.Commands;

public class DeleteNoteCommand : IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
}