using System.Net;
using FluentAssertions;
using Integration.Application.Tests.Environment;
using Lacuna.Application.DTO;

namespace Integration.Application.Tests;

public class ServicesTests : IntegrationTestEnvironment
{
    private readonly HttpClient _client;
    public ServicesTests()
    {
        _client = GenerateClient();
    }
    
    [Fact]
    public async Task Post_RegisterANewUserInApp_ShouldReturnStatusCode200()
    {
        // Arrange

        // Act
        var register = await Register(_client, new CreateUserDto
        {
            Username = "sampleusername",
            Email = "sampleemail@example.com",
            Password = "workworkPlease123@",
            ConfirmPassword = "workworkPlease123@"
        });

        // Assert
        register.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_AttemptToRegisterAnExistentUser_ShouldReturnStatusCode409()
    {
        // Arrange
        
        // Act
        var register = await Register(_client, new CreateUserDto
        {
            Username = "sampleusername",
            Email = "sampleuser@email.com",
            Password = "workworkPlease123@",
            ConfirmPassword = "workworkPlease123@"
        });
        
        // Assert
        register.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Post_LoginOfAUserRegistered_ShouldReturnStatusCode200()
    {
        // Arrange
        
        // Act
        var login = await Login(_client, new LoginUserDto
        {
            Username = "sampleuser",
            Password = "samplePassw0rd@123"
        });
        
        // Assert
        login.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_LoginOfAnIncorrectUser_ShouldReturnStatusCode401()
    {
        // Arrange
        
        // Act
        var login = await Login(_client, new LoginUserDto
        {
            Username = "sampleuser",
            Password = "samplePassword@123"
        });
        
        // Assert
        login.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_LoginOfAnInexistentUser_ShouldReturnStatusCode404()
    {
        // Arrange
        
        // Act
        var login = await Login(_client, new LoginUserDto
        {
            Username = "NonExistentUser",
            Password = "something3131312@32"
        });
        
        // Assert
        login.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}