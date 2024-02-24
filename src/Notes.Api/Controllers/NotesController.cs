using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Commands;
using Notes.Application.Queries;
using Notes.Domain;
using Notes.Domain.Exceptions;
using Notes.Infraestructure.Exceptions;

namespace Notes.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<Note>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List(CancellationToken cancellationToken) 
    {
        try
        {
            var response = await _mediator.Send(new GetAllNotesQuery(), cancellationToken);
            return Ok(response);
        }
        catch (InvalidNoteException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create([FromBody] CreateNoteCommand model, CancellationToken cancellationToken) 
    {
        try
        {
            var note = await _mediator.Send(model, cancellationToken);
            return Ok(note);
        }
        catch (InvalidNoteException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateNoteCommand model, CancellationToken cancellationToken) 
    {
        try
        {
            var note = await _mediator.Send(model, cancellationToken);
            return Ok(note);
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidNoteException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete()]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken) 
    {
        try
        {
            await _mediator.Send(new DeleteNoteCommand() { Id = id }, cancellationToken);
            return Ok();
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidNoteException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}