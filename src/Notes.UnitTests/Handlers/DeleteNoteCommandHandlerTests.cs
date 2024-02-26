using Moq;
using Xunit;
using Notes.Application.Commands;
using Notes.Application.Handlers;
using Notes.Application.Repositories;

namespace Notes.UnitTests.Handlers;

public class DeleteNoteCommandHandlerTests
{
    [Fact()]
    public async Task ShouldCallDeleteAsyncFromRepositoryWithCorrectNoteId()
    {
        var repositoryMock = new Mock<INoteRepository>();
        var sut = new DeleteNoteCommandHandler(repositoryMock.Object);
        var command = new DeleteNoteCommand() { Id = Guid.NewGuid().ToString() };
        await sut.Handle(command, CancellationToken.None);
        var isMethodCalledWithCorrectId = repositoryMock.Invocations
            .Where(invocation => invocation.Method.Name.Equals(nameof(INoteRepository.DeleteByIdAsync)))
            .Where(invocation => invocation.Arguments.Any(arg => arg.Equals(command.Id))).Any();
        Assert.True(isMethodCalledWithCorrectId);
    }
}
