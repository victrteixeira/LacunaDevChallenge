using System.Security.Authentication;
using Lacuna.Application.DTO;
using Lacuna.Application.Interfaces;
using Lacuna.Application.Responses;
using Lacuna.Application.Services;
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
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(406, ModelState);

            var user = await _auth.Register(userDto);
            return Ok(user);
        }
        catch (Exception e) when (e is AuthenticationException)
        {
            if (e is AuthenticationException)
            {
                return StatusCode(409, new CreateUserResponse
                {
                    Code = "Error",
                    Message = e.Message
                });
            }
            
            return StatusCode(500, new CreateUserResponse
            {
                Code = "Error",
                Message = e.Message
            });
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogInUser([FromBody] LoginUserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(406, ModelState);

            var token = await _auth.Login(userDto);
            return Ok(token);
        }
        catch (Exception e) when (e is NullReferenceException || e is AuthenticationException)
        {
            if (e is NullReferenceException)
            {
                return StatusCode(404, new TokenResponse
                {
                    AccessToken = null,
                    Code = "Error",
                    Message = "User not found or don't exist."
                });
            }

            return Unauthorized();
        }
    }
}