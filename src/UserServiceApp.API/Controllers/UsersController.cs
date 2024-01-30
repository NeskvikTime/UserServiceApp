using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApp.Application.Users.DeleteUser;
using UserServiceApp.Application.Users.GetUserDatas;
using UserServiceApp.Application.Users.Login;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class UsersController(ISender _sender) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
    {
        var query = new GetUserDatasQuery(null);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("get/{userId:guid}")]
    public async Task<IActionResult> GetUserDatas([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserDatasQuery(userId);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPut("update/{userId:guid}")]
    public async Task<IActionResult> UpdateUserData([FromRoute] Guid userId, [FromQuery] string newAcceptLanguageCulture, UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(
            userId,
            request.UserName,
            request.FullName,
            request.Email,
            request.MobileNumber,
            newAcceptLanguageCulture,
            request.isAdmin,
            request.NewPassword);

        var result = await _sender.Send(command, CancellationToken.None);
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.UserName,
            request.FullName,
            request.Email,
            request.Password,
            request.MobileNumber);

        var result = await _sender.Send(command, CancellationToken.None);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        var result = await _sender.Send(command, CancellationToken.None);
        return Ok(result);
    }

    [HttpDelete("delete/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var request = new DeleteUserCommand(userId);

        await _sender.Send(request, CancellationToken.None);
        return NoContent();
    }
}