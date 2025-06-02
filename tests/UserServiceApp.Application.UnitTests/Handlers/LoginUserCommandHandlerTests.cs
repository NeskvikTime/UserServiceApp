using FluentAssertions;
using MediatR;
using NSubstitute;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Application.Users.Login;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Domain.Common.Interfaces;
using UserServiceApp.Tests.Shared.Builders;

namespace UserServiceApp.Application.UnitTests.Handlers;
public class LoginUserCommandHandlerTests
{
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IJwtTokenGenerator _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>(); // Added missing dependency
    private readonly IRequestHandler<LoginUserCommand, AuthenticationResult> _handler;

    public LoginUserCommandHandlerTests()
    {
        _handler = new LoginUserCommandHandler(_userService, Substitute.For<IRefreshTokenRepository>(), _unitOfWork, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthenticationResultWithToken()
    {
        // Arrange
        var command = new LoginUserCommand("user@example.com", "ValidPassword123");
        var user = new UserBuilder()
                        .WithUsername("username")
                        .WithFullName("FullName")
                        .WithEmail("user@example.com")
                        .WithMobileNumber("1234567890")
                        .WithLanguage("English")
                        .WithCulture("en-US")
                        .WithPasswordHash("hashedPassword") // Assuming your builder has a method for this, or adjust as necessary
                        .WithIsAdmin(false)
                        .Build();
        var jwtToken = "GeneratedJWTToken";

        _userService.LoginUserAsync(command.Email, command.Password, Arg.Any<CancellationToken>()).Returns(user);
        _jwtTokenGenerator.GenerateToken(user).Returns(jwtToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserResponse.Should().NotBeNull();
        result.UserResponse.Email.Should().Be(user.Email);
        result.Token.Should().Be(jwtToken);

        result.UserResponse.Id.Should().Be(user.Id);
        result.UserResponse.Username.Should().Be(user.Username);
        result.UserResponse.FullName.Should().Be(user.FullName);
        result.UserResponse.Email.Should().Be(user.Email);
        result.UserResponse.MobileNumber.Should().Be(user.MobileNumber);
        result.UserResponse.Language.Should().Be(user.Language);
        result.UserResponse.Culture.Should().Be(user.Culture);
        result.UserResponse.IsAdmin.Should().Be(user.IsAdmin);
    }
}
