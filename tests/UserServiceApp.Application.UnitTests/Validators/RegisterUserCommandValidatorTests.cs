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
    public async Task Should_Have_Error_When_UserName_Is_Empty()
    {
        var command = new RegisterUserCommand(string.Empty, "Full Name", "email@example.com", "+1234567890", "Password123!");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public async Task Should_Have_Error_When_UserName_Exceeds_Maximum_Length()
    {
        var command = new RegisterUserCommand(new string('a', 21), "Full Name", "email@example.com", "+1234567890", "Password123!"); // 21 characters
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public async Task Should_Have_Error_When_UserName_Already_Exists()
    {
        _usersRepository.UsernameIsUniqueAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false)); // Username already exists

        var command = new RegisterUserCommand("ExistingUsername", "Full Name", "email@example.com", "+1234567890", "Password123!");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName).WithErrorMessage("Username already exists");
    }

    [Fact]
    public async Task Should_Have_Error_When_Email_Is_Invalid_FormatAsync()
    {
        var command = new RegisterUserCommand("Username", "Full Name", "invalidemail", "+1234567890", "Password123!");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task Should_Have_Error_When_Email_Already_Exists()
    {
        _usersRepository.EmailIsUniqueAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false)); // Email already exists

        var command = new RegisterUserCommand("Username", "Full Name", "existing@example.com", "+1234567890", "Password123!");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Email already exists");
    }

    [Fact]
    public async Task Should_Have_Error_When_Password_Does_Not_Meet_RequirementsAsync()
    {
        var command = new RegisterUserCommand("Username", "Full Name", "user@example.com", "+1234567890", "weak");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

}
