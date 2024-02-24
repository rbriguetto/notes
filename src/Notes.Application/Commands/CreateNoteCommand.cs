using MediatR;
using Notes.Domain;

namespace Notes.Application.Commands;

public class CreateNoteCommand : IRequest<Note>
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}