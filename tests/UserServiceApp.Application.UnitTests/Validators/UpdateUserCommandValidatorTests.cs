using FluentValidation.TestHelper;
using NSubstitute;
using UserServiceApp.Tests.Shared.Builders;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.UpdateUserData;

namespace UserServiceApp.Application.UnitTests.Validators;
public class UpdateUserCommandValidatorTests
{
    private readonly IUsersRepository _usersRepository = Substitute.For<IUsersRepository>();
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator(_usersRepository);
    }

    [Fact]
    public async Task UserName_ExceedingMaximumLength_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommandBuilder()
                            .WithUserName(new string('a', 21)) // 21 characters
                            .Build();

        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.UserName);
    }

    [Fact]
    public async Task Email_InvalidFormat_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommandBuilder()
                            .WithEmail("invalidEmail")
                            .Build();

        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public async Task MobileNumber_InvalidFormat_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommandBuilder()
                            .WithMobileNumber("invalidNumber")
                            .Build();

        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.MobileNumber);
    }

    [Fact]
    public async Task NewPassword_InvalidFormat_ShouldHaveValidationError()
    {
        var command = new UpdateUserCommandBuilder()
                            .WithNewPassword("weak") // Does not meet complexity requirements
                            .Build();

        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
    }

    [Fact]
    public async Task Validator_ShouldNotHaveAnyValidationErrors_WhenCommandIsValid()
    {
        var command = new UpdateUserCommandBuilder().WithNewPassword("newPassword-1234!").Build();

        // Ensure the username and email are considered unique for this test
        _usersRepository.UsernameIsUniqueAsync(command.UserId, command.UserName, Arg.Any<System.Threading.CancellationToken>()).Returns(Task.FromResult(true));
        _usersRepository.EmailIsUniqueAsync(command.UserId, command.Email, Arg.Any<System.Threading.CancellationToken>()).Returns(Task.FromResult(true));

        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
