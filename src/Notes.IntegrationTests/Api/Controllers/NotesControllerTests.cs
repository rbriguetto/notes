using System.Net;
using Xunit;
using Notes.Application.Commands;
using Notes.Application.Repositories;
using Notes.Domain;
using Notes.Infraestructure.Exceptions;

namespace Notes.IntegrationTests.Controllers;

public class NotesControllerTests : IClassFixture<IntegrationTestsApplicationFactory>
{
    private readonly IntegrationTestsApplicationFactory _factory;

    public NotesControllerTests(IntegrationTestsApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact()]
    public async Task ListMethodShouldReturnDataFromRepository()
    {
        const int NOTE_COUNT = 10;
        var serviceProvider = _factory.Services.CreateScope().ServiceProvider;
        var noteRepository = serviceProvider.GetRequiredService<INoteRepository>();

        for(int i = 0; i < NOTE_COUNT; i++)
        {
            await noteRepository.CreateAsync(new Note() { Id = Guid.NewGuid().ToString(), 
                Text = Guid.NewGuid().ToString(), Title = Guid.NewGuid().ToString() });
        }

        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/Notes/List");
        response.EnsureSuccessStatusCode();
 
        var notes = await response.Content.ReadFromJsonAsync<Note[]>();
        Assert.Equal(NOTE_COUNT, notes?.Count());
    }

    [Fact()]
    public async Task CreateNoteApiShouldSaveNoteOnRepository()
    {
        var command = new CreateNoteCommand() { Title = Guid.NewGuid().ToString(), Text = Guid.NewGuid().ToString() };
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Notes/Create", command);
        response.EnsureSuccessStatusCode();
        var note = await response.Content.ReadFromJsonAsync<Note>();
        var serviceProvider = _factory.Services.CreateScope().ServiceProvider;
        var noteRepository = serviceProvider.GetRequiredService<INoteRepository>();
        var noteFromRepository = await noteRepository.GetByIdAsync(note.Id);
        Assert.Equal(command.Title, noteFromRepository.Title);
        Assert.Equal(command.Text, noteFromRepository.Text);
    }

    [Fact()]
    public async Task UpdateNoteApiShouldUpdateNoteOnRepository()
    {
        var command = new CreateNoteCommand() { Title = Guid.NewGuid().ToString(), Text = Guid.NewGuid().ToString() };
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Notes/Create", command);
        response.EnsureSuccessStatusCode();
        var note = await response.Content.ReadFromJsonAsync<Note>();

        var updatedNote = new Note() { Id = note.Id, Title = Guid.NewGuid().ToString(), Text = Guid.NewGuid().ToString() };
        response = await client.PostAsJsonAsync("/api/Notes/Update", updatedNote);
        response.EnsureSuccessStatusCode();

        var serviceProvider = _factory.Services.CreateScope().ServiceProvider;
        var noteRepository = serviceProvider.GetRequiredService<INoteRepository>();
        var noteFromRepository = await noteRepository.GetByIdAsync(note.Id);
        Assert.Equal(updatedNote.Title, noteFromRepository.Title);
        Assert.Equal(updatedNote.Text, noteFromRepository.Text);
    }

    [Fact()]
    public async Task UpdateNoteApiShouldReturn404WhenNoteDotNotExists()
    {
        var client = _factory.CreateClient();
        var note = new Note() { Id = "INVALID-ID", Title = Guid.NewGuid().ToString(), Text = Guid.NewGuid().ToString() };
        var response = await client.PostAsJsonAsync("/api/Notes/Update", note);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact()]
    public async Task DeleteNoteApiShouldDeleteWhenNoteExists()
    {
        var command = new CreateNoteCommand() { Title = Guid.NewGuid().ToString(), Text = Guid.NewGuid().ToString() };
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/Notes/Create", command);
        response.EnsureSuccessStatusCode();
        var note = await response.Content.ReadFromJsonAsync<Note>();

        response = await client.DeleteAsync($"/api/Notes/Delete?id={note.Id}");
        response.EnsureSuccessStatusCode();

        var serviceProvider = _factory.Services.CreateScope().ServiceProvider;
        var noteRepository = serviceProvider.GetRequiredService<INoteRepository>();
        await Assert.ThrowsAsync<NoteNotFoundException>(async () => await noteRepository.GetByIdAsync(note.Id));
    }
}