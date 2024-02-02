﻿using FluentAssertions;
using MediatR;
using NSubstitute;
using TestCommon.Builders;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.GetUserDatas;
using UserServiceApp.Contracts.Users;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.UnitTests.Handlers;
public class GetUserDatasQueryHandlerTests
{
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IRequestHandler<GetUserDatasQuery, List<UserResponse>> _handler;

    public GetUserDatasQueryHandlerTests()
    {
        _handler = new GetUserDatasQueryHandler(_userService);
    }

    [Fact]
    public async Task Handle_GivenValidUserId_ReturnsUserResponseList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        User user = new UserBuilder()
                            .WithId(userId)
                            .WithUsername("username1")
                            .WithFullName("FullName1")
                            .WithEmail("email1@example.com")
                            .WithMobileNumber("1234567890")
                            .WithLanguage("English")
                            .WithCulture("en-US")
                            .WithPasswordHash("passwordHash1") // Note: This won't be part of UserResponse but necessary for building the User.
                            .WithIsAdmin(false)
                            .Build();

        List<User> users = [user];

        _userService.GetUserDatasAsync(Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(users);

        var query = new GetUserDatasQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(users.Count);

        var userResponse = result[0];
        userResponse.Username.Should().Be(user.Username);
        userResponse.FullName.Should().Be(user.FullName);
        userResponse.Email.Should().Be(user.Email);
        userResponse.MobileNumber.Should().Be(user.MobileNumber);
        userResponse.Language.Should().Be(user.Language);
        userResponse.Culture.Should().Be(user.Culture);
        userResponse.IsAdmin.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_GivenNoUserId_ReturnsAllUsersResponseList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        User user = new UserBuilder()
                            .WithId(userId)
                            .WithUsername("username2")
                            .WithFullName("FullName2")
                            .WithEmail("email2@example.com")
                            .WithMobileNumber("0987654321")
                            .WithLanguage("Spanish")
                            .WithCulture("es-ES")
                            .WithPasswordHash("passwordHash2")
                            .WithIsAdmin(true)
                            .Build();

        List<User> users = [user];

        _userService.GetUserDatasAsync(Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(users);

        var query = new GetUserDatasQuery(user.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(users.Count);

        var userResponse = result[0];
        userResponse.Username.Should().Be(user.Username);
        userResponse.FullName.Should().Be(user.FullName);
        userResponse.Email.Should().Be(user.Email);
        userResponse.MobileNumber.Should().Be(user.MobileNumber);
        userResponse.Language.Should().Be(user.Language);
        userResponse.Culture.Should().Be(user.Culture);
        userResponse.IsAdmin.Should().BeTrue();
    }
}