using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.Login;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.Application.UnitTests.Validators;
public class LoginUserCommandValidatorTests
{
    private readonly IUsersRepository _usersRepository = Substitute.For<IUsersRepository>();
    private readonly LoginUserCommandValidator _validator;

    public LoginUserCommandValidatorTests()
    {
        _validator = new LoginUserCommandValidator(_usersRepository);
    }

    [Fact]
    public async Task Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var command = new LoginUserCommand(string.Empty, "ValidPassword123");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task Should_Have_Error_When_Email_Is_Not_Valid_Format()
    {
        // Arrange
        var command = new LoginUserCommand("notanemail", "ValidPassword123");


        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task Should_Have_Error_When_User_Does_Not_Exist_By_Email()
    {
        // Arrange
        var command = new LoginUserCommand(
            "user@doesnotexist.com",
            "Password123");

        _usersRepository.UserByEmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // Act
        Func<Task> act = async () => await _validator.TestValidateAsync(command);

        // Assert
        await act.Should().ThrowAsync<AuthorizationException>();
    }

    [Fact]
    public async Task Should_Have_Error_When_Password_Is_Empty()
    {
        // Arrange
        var command = new LoginUserCommand("user@example.com", string.Empty);

        _usersRepository.UserByEmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        _usersRepository.UserByEmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        var command = new LoginUserCommand("validuser@example.com", "ValidPassword123");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
