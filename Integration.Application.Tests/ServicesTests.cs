using System.Net;
using FluentAssertions;
using Integration.Application.Tests.Environment;
using Lacuna.Application.DTO;

namespace Integration.Application.Tests;

public class ServicesTests : IntegrationTestEnvironment
{
    [Fact]
    public async Task Post_RegisterANewUserInApp_ShouldReturnStatusCode200()
    {
        // Arrange
        var client = GenerateClient();
        
        // Act
        var register = await Register(client, new CreateUserDto
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
    public async Task Post_LoginOfAUserRegistered_ShouldReturnStatusCode200()
    {
        // Arrange
        var client = GenerateClient();
        
        // Act
        var login = await Login(client, new LoginUserDto
        {
            Username = "sampleuser",
            Password = "samplePassw0rd@123"
        });
        
        // Assert
        login.Should().HaveStatusCode(HttpStatusCode.OK);
    }
}