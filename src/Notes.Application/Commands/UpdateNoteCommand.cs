using MediatR;
using Notes.Domain;

namespace Notes.Application.Commands;

public class UpdateNoteCommand: IRequest<Note>
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}