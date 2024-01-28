using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApp.Application.Users.DeleteUser;
using UserServiceApp.Application.Users.GetUserData;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Contracts.Common;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ISender _sender) : ControllerBase
{
    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetUserData(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserDataQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(AuthenticationResult.MapToAuthResponse(result));
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserData(Guid id, UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(
            id,
            request.UserName,
            request.FullName,
            request.Email,
            request.MobileNumber,
            request.Language,
            request.Culture,
            request.isAdmin);

        var result = await _sender.Send(command, CancellationToken.None);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var result = await _sender.Send(request, CancellationToken.None);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _sender.Send(request, CancellationToken.None);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("delete/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var request = new DeleteUserCommand(userId);

        await _sender.Send(request, CancellationToken.None);
        return NoContent();
    }
}