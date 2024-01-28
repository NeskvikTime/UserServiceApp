using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApp.API.Helper;
using UserServiceApp.Application.Users.DeleteUser;
using UserServiceApp.Application.Users.GetUserDatas;
using UserServiceApp.Application.Users.RegisterUser;
using UserServiceApp.Application.Users.UpdateUserData;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ISender _sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserDatas([FromQuery] Guid? userId, CancellationToken cancellationToken)
    {
        var query = new GetUserDatasQuery(userId);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateUserData(Guid id, [FromQuery] string newAcceptLanguageCulture, UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(
            id,
            request.UserName,
            request.FullName,
            request.Email,
            request.MobileNumber,
            newAcceptLanguageCulture,
            request.isAdmin);

        var result = await _sender.Send(command, CancellationToken.None);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var culture = Request.HttpContext.GetCultureFromRequest();

        var command = new RegisterUserCommand(
            request.UserName,
            request.FullName,
            request.Email,
            request.Password,
            request.MobileNumber,
            culture);

        var result = await _sender.Send(command, CancellationToken.None);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _sender.Send(request, CancellationToken.None);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("Delete/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var request = new DeleteUserCommand(userId);

        await _sender.Send(request, CancellationToken.None);
        return NoContent();
    }
}