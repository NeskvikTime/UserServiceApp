using FluentAssertions;
using MediatR;
using NSubstitute;
using UserServiceApp.Tests.Shared.Builders;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.UnitTests.Handlers;
public class UpdateUserCommandHandlerTests
{
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IRequestHandler<UpdateUserCommand, UserResponse> _handler;

    public UpdateUserCommandHandlerTests()
    {
        _handler = new UpdateUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesUserAndReturnsUserResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommandBuilder()
            .WithUserId(userId)
            .WithUserName("NewUsername")
            .WithFullName("New Full Name")
            .WithEmail("newemail@example.com")
            .WithMobileNumber("+11234567890")
            .WithNewCulture("en-US")
            .IsAdmin(true)
            .WithNewPassword("NewPassword123!")
            .Build();

        var updatedUser = new User(
            command.UserName,
            command.FullName,
            command.Email,
            command.MobileNumber,
            "English", // Assuming UserService sets language based on culture
            command.NewCulture,
            command.IsAdmin,
            userId);

        _userService.UpdateUserAsync(Arg.Any<UpdateUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(updatedUser));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Username.Should().Be(command.UserName);
        result.FullName.Should().Be(command.FullName);
        result.Email.Should().Be(command.Email);
        result.MobileNumber.Should().Be(command.MobileNumber);
        result.Language.Should().Be("English");
        result.Culture.Should().Be(command.NewCulture);
        result.IsAdmin.Should().Be(command.IsAdmin);

        await _userService.Received(1).UpdateUserAsync(Arg.Is<UpdateUserCommand>(cmd =>
            cmd.UserId == command.UserId &&
            cmd.UserName == command.UserName &&
            cmd.Email == command.Email),
            Arg.Any<CancellationToken>());
    }
}
