using FluentValidation.TestHelper;
using NSubstitute;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.RegisterUser;

namespace UserServiceApp.Application.UnitTests.Validators;
public class RegisterUserCommandValidatorTests
{
    private readonly IUsersRepository _usersRepository = Substitute.For<IUsersRepository>();
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTests()
    {
        _validator = new RegisterUserCommandValidator(_usersRepository);
    }

    [Fact]
    public async Task ValidateAsync_WithEmptyUserName_ShouldHaveValidationErrorForUserName()
    {
        // Arrange
        var command = new RegisterUserCommand(
            string.Empty,
            "Full Name",
            "email@example.com",
            "+1234567890",
            "Password123!");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public async Task ValidateAsync_WithUserNameExceedingMaximumLength_ShouldHaveValidationErrorForUserName()
    {
        var command = new RegisterUserCommand(new string('a', 21), "Full Name", "email@example.com", "+1234567890", "Password123!"); // 21 characters
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public async Task ValidateAsync_WithNonUniqueUserName_ShouldHaveValidationErrorForUserName()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "ExistingUsername",
            "Full Name",
            "email@example.com",
            "+1234567890",
            "Password123!");

        _usersRepository.UsernameIsUniqueAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false)); // Username already exists

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidEmailFormat_ShouldHaveValidationErrorForEmail()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "Username",
            "Full Name",
            "invalidemail",
            "+1234567890",
            "Password123!");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task ValidateAsync_WithNonUniqueEmail_ShouldHaveValidationErrorForEmail()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "Username",
            "Full Name",
            "existing@example.com",
            "+1234567890",
            "Password123!");

        _usersRepository.EmailIsUniqueAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false)); // Email already exists

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task ValidateAsync_WithPasswordNotMeetingRequirements_ShouldHaveValidationErrorForPassword()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "Username",
            "Full Name",
            "user@example.com",
            "+1234567890",
            "weak");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
