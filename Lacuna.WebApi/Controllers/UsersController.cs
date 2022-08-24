using System.Security.Authentication;
using Lacuna.Application.DTO;
using Lacuna.Application.Interfaces;
using Lacuna.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Lacuna.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthentication _auth;
    public UsersController(IAuthentication auth)
    {
        _auth = auth;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateUser(CreateUserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(406, ModelState);

            var user = await _auth.Register(userDto);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, new CreateUserResponse
            {
                Code = "Error",
                Message = e.Message
            });
        }
    }
}