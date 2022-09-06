using Unit.Application.Tests.Fixtures;

namespace Unit.Application.Tests;

public class ServicesTests
{
    private readonly IAuthentication _sut;

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenRequest> _tokenRequestMock;
    private readonly Mock<IPasswordValidation> _pwdValidationMock;

    public ServicesTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenRequestMock = new Mock<ITokenRequest>();
        _pwdValidationMock = new Mock<IPasswordValidation>();

        _sut = new Authentication(_userRepositoryMock.Object, _tokenRequestMock.Object, _pwdValidationMock.Object);
    }

    [Fact(DisplayName = "Register of An Valid User in Database")]
    [Trait("Authentication", "Register")]
    public async Task Authentication_RegisterAValidUser_ShouldReturnAnCreateUserResponse()
    {
        // Arrange
        var userDto = RepoFixture.ValidCreateUserDto();
        var createdResponse = new CreateUserResponse { Code = "Success", Message = "User created successfully." };

        _userRepositoryMock.Setup(x => x.GetEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null).Verifiable();

        User user = new(userDto.Username, userDto.Email, userDto.Password);
        _userRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(user).Verifiable();
        
        // Act
        var res = await _sut.Register(userDto);
        
        // Assert
        _userRepositoryMock.Verify(x => x.CreateUserAsync(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetEmailAsync(It.IsAny<string>()), Times.Once);

        res.Should().BeOfType<CreateUserResponse>()
            .And.BeEquivalentTo(createdResponse);
    }

    [Fact(DisplayName = "Authentication of An Already Registered User")]
    [Trait("Authentication", "LogIn")]
    public async Task Authentication_LogInOfAValidUser_ShouldReturnAnTokenResponse()
    {
        // Arrange
        var loginDto = RepoFixture.ValidLoginDto();
        var validUser = new User(loginDto.Username, new Internet().Email(), loginDto.Password);
        var token = new TokenResponse { AccessToken = "somestringencodedforjwttokenresponse", Code = "Success" };

        _userRepositoryMock.Setup(x => x.GetUsernameAsync(loginDto.Username))
            .ReturnsAsync(validUser).Verifiable();
        
        _pwdValidationMock.Setup(x => x.IsValid(loginDto, validUser.HashedPassword))
            .Returns(true).Verifiable();

        _tokenRequestMock.Setup(x => x.GenerateToken(validUser))
            .ReturnsAsync(token).Verifiable();
        
        // Act
        var res = await _sut.Login(loginDto);
        
        // Assert
        _userRepositoryMock.Verify(x => x.GetUsernameAsync(loginDto.Username),
            Times.Exactly(1));
        
        _pwdValidationMock.Verify(x => x.IsValid(loginDto, validUser.HashedPassword), Times.Once);
        
        _tokenRequestMock.Verify(x => x.GenerateToken(validUser),
            Times.Exactly(1));

        res.Should().BeEquivalentTo(token);
    }

    [Fact(DisplayName = "Attempt to Register An Existent User in Database")]
    [Trait("Authentication", "Register")]
    public async Task Authentication_TryToRegisterInvalidUser_ShouldThrowAuthenticationException()
    {
        // Arrange
        var invalidUserDto = RepoFixture.ValidCreateUserDto();
        User existentUser = new(invalidUserDto.Username, invalidUserDto.Email, invalidUserDto.Password);

        _userRepositoryMock.Setup(x => x.GetEmailAsync(invalidUserDto.Email))
            .ReturnsAsync(existentUser);
        
        // Act
        Func<Task> res = async () => await _sut.Register(invalidUserDto);
        
        // Assert
        await res.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Email already registered.");
    }

    [Fact(DisplayName = "Attempt to Login A Non-Existent User")]
    [Trait("Authentication", "Login")]
    public async Task Authentication_TryToLoginNonExistentUser_ShouldThrowNullReferenceException()
    {
        // Arrange
        var nonExistentUser = RepoFixture.ValidLoginDto();

        _userRepositoryMock.Setup(x => x.GetUsernameAsync(nonExistentUser.Username))
            .ReturnsAsync((User?)null);
        
        // Act
        Func<Task> res = async () => await _sut.Login(nonExistentUser);
        
        // Assert
        await res.Should().ThrowAsync<NullReferenceException>()
            .WithMessage("User not found.");
    }
}