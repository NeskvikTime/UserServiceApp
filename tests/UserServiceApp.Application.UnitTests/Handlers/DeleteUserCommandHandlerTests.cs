using MediatR;
using NSubstitute;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.DeleteUser;

namespace UserServiceApp.Application.UnitTests.Handlers;
public class DeleteUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly IRequestHandler<DeleteUserCommand, Unit> _handler;

    public DeleteUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new DeleteUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_CallsDeleteUserAsyncOnUserService()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userService.Received(1).DeleteUserAsync(userId, Arg.Any<CancellationToken>());
    }
}
