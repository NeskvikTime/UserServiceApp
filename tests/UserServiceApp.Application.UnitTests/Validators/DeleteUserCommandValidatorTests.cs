﻿using FluentValidation.TestHelper;
using NSubstitute;
using TestCommon.Builders;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.DeleteUser;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.UnitTests.Validators;

public class DeleteUserCommandValidatorTests
{
    private readonly IUsersRepository _usersRepository;
    private readonly DeleteUserCommandValidator _validator;

    public DeleteUserCommandValidatorTests()
    {
        _usersRepository = Substitute.For<IUsersRepository>();
        _validator = new DeleteUserCommandValidator(_usersRepository);
    }

    [Fact]
    public async Task ValidateUserExistsAsync_UserExists_ShouldPassValidation()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _usersRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(new UserBuilder().Build());
        var command = new DeleteUserCommand(userId);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task ValidateUserExistsAsync_UserDoesNotExist_ShouldFailValidation()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _usersRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((User)null);
        var command = new DeleteUserCommand(userId);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId).WithErrorMessage($"User with id: {userId} does not exist.");
    }
}